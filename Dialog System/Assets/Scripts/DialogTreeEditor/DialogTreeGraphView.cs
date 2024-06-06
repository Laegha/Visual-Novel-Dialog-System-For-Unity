using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
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
        evt.menu.AppendAction($"[{Type.GetType("DialogNode")}]", (a) => CreateNode(null));
    }

    public void PopulateView(DialogTree dialogTree)
    {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        nodeViews.Clear();

        if (dialogTree == null)
            return;

        bool generatingNodes = true;
        Dialog nextNodeDialog = dialogTree.initialDialog;
        while(generatingNodes)
        {

        }
        //for (int i = 0; i < dialogTree.Dialogs.Count; i++)
        //{
        //    DialogNodeView outputNodeView = CreateNode(i.ToString(), dialogTree.Dialogs[i.ToString()]).View;
        //    int j = 1;
        //    foreach (KeyValuePair<Dialog, List<DialogChangeCondition>> nextDialog in dialogTree.Dialogs[i.ToString()].possibleNextDialogs)
        //    {
        //        DialogNodeView inputNodeView = CreateNode(nextDialog.Key).View;
        //        CreateEdge(outputNodeView, inputNodeView, nextDialog.Value.ToArray());
        //        j++;
        //    }
        //}
    }

    GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                DialogNodeView nodeView = elem as DialogNodeView;
                if (nodeView != null)
                {
                    nodeView.RemoveFromHierarchy();
                    editor.RemoveNode(nodeView.node);

                    nodeViews.Remove(nodeView);
                    if (nodeView.node.IsInitial && nodeViews.Count > 0)
                    {
                        nodeViews[0].inputContainer.Remove(nodeViews[0].input);
                        nodeViews[0].node.IsInitial = true;
                        editor.currTree.initialDialog = nodeViews[0].node.Dialog;
                    }
                }

                DialogEdge dialogEdge = elem as DialogEdge;
                if (dialogEdge != null)
                {
                    dialogEdge.RemoveFromHierarchy();
                    dialogEdge.OnRemoved();
                    Debug.Log("Removed edge");
                }

            });
        if (graphViewChange.edgesToCreate != null)
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                DialogEdge dialogEdge = edge as DialogEdge;
                if (dialogEdge != null)
                {
                    dialogEdge.Start(editor.inspectorView);
                }
            });


        return graphViewChange;
    }

    DialogNode CreateNode(Dialog value)
    {
        if (!editor.IsTreeSet())
        {
            Debug.LogError("Tree must not be null");
            return null;
        }

        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.Dialog = value;
        node.IsInitial = nodeViews.Count == 0 ? true : false ;

        editor.AddNode(node);

        CreateNodeView(node);

        return node;
    }

    void CreateNodeView(DialogNode node)
    {
        DialogNodeView newNodeView = new DialogNodeView();
        newNodeView.node = node != null ? node : ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        newNodeView.treeEditor = editor;
        newNodeView.title = "New Dialog";
        newNodeView.Start();

        AddElement(newNodeView);
        nodeViews.Add(newNodeView);
    }

    void CreateEdge(DialogNodeView outputNode, DialogNodeView inputNode, DialogChangeCondition[] dialogChangeConditions)
    {
        DialogEdge dialogEdge = outputNode.output.ConnectTo(inputNode.input) as DialogEdge;
        dialogEdge.Start(editor.inspectorView);
        dialogEdge.dialogConnection._connectionChangeConditions = dialogChangeConditions;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
    }
}