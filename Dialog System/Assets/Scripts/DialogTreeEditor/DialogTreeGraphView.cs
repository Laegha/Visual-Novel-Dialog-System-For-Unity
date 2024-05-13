using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Pipeline;

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
        evt.menu.AppendAction($"[{Type.GetType("DialogNode")}]", (a) => CreateNode(generatedNodes, null));
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

        for (int i = 0; i < dialogTree.Dialogs.Count; i++)
        {
            DialogNodeView outputNodeView = CreateNode(i, dialogTree.Dialogs[i]).View;
            int j = 1;
            foreach (KeyValuePair<Dialog, List<DialogChangeCondition>> nextDialog in dialogTree.Dialogs[i].possibleNextDialogs)
            {
                DialogNodeView inputNodeView = CreateNode(i + j, nextDialog.Key).View;
                CreateEdge(outputNodeView, inputNodeView, nextDialog.Value.ToArray());
                j++;
            }
        }
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

                    nodeViews.Remove(nodeView);
                    if (nodeView.node.DialogIndex == 0)
                        nodeViews[0].Remove(nodeViews[0].inputContainer);
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

    DialogNode CreateNode(int key, Dialog value)
    {
        if (!editor.IsTreeRefreshed())
            return null;
        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.Dialog = value;
        node.DialogIndex = key;
        editor.AddNode(node);
        CreateNodeView(node);
        generatedNodes++;
        return node;
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

    void CreateEdge(DialogNodeView outputNode, DialogNodeView inputNode, DialogChangeCondition[] dialogChangeConditions)
    {
        DialogEdge dialogEdge = outputNode.output.ConnectTo(inputNode.input) as DialogEdge;
        dialogEdge.Start(editor.inspectorView);
        dialogEdge.dialogConnection.dialogChangeConditions = dialogChangeConditions;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
    }
}
