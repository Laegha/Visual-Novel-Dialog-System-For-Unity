using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class DialogTreeGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeGraphView, GraphView.UxmlTraits> { }

    public DialogTreeEditor editor;

    List<DialogNodeView> nodeViews = new List<DialogNodeView>();

    public DialogTreeGraphView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/DialogTreeEditor/DialogTreeEditor.uss");
        styleSheets.Add(styleSheet);

    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction($"[{Type.GetType("DialogNode")}]", (a) => CreateNode(generatedNodes.ToString(), null));
    }

    int generatedNodes = 0;
    public void PopulateView(DialogTree dialogTree)
    {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        
        generatedNodes = 0;
     
        if (dialogTree == null)
            return; 

        foreach (KeyValuePair<string, Dialog> dialog in dialogTree.Dialogs)
            CreateNode(dialog.Key, dialog.Value);


    }
    
    GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if(graphViewChange.elementsToRemove != null)
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                DialogNodeView nodeView = elem as DialogNodeView;
                if(nodeView != null)
                {
                    nodeView.RemoveFromHierarchy();
                    editor.RemoveNode(nodeView.node);
                }

                DialogEdge dialogEdge = elem as DialogEdge;
                if(dialogEdge != null)
                {
                    dialogEdge.RemoveFromHierarchy();
                    dialogEdge.OnRemoved();
                }

            });
        if (graphViewChange.edgesToCreate != null)
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                DialogEdge dialogEdge = edge as DialogEdge;
                if(dialogEdge != null)
                {
                    dialogEdge.Start(editor.inspectorView);
                }
            });


        return graphViewChange;
    }

    void CreateNode(string key, Dialog value)
    {
        if (!editor.IsTreeRefreshed())
            return;
        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.Dialog = value;
        node.DialogIndex = key;
        editor.AddNode(node);
        CreateNodeView(node);
        generatedNodes++;
    }

    void CreateNodeView(DialogNode node)
    {
        DialogNodeView newNodeView = new DialogNodeView();
        newNodeView.node = node != null ? node : ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        newNodeView.treeEditor = editor;
        newNodeView.title = "New DialogNode";
        newNodeView.Start();
        
        AddElement(newNodeView);
        nodeViews.Add(newNodeView);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
    }
}
