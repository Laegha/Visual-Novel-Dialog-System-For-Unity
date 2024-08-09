using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DialogTreeGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeGraphView, GraphView.UxmlTraits> { }

    public DialogTreeEditor editor;

    public bool isPopulating;

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
        evt.menu.AppendAction($"[{Type.GetType("DialogNode")}]", (a) =>{ 
            DialogNode createdNode = CreateNode(new DialogData(null, "")); if(createdNode.IsInitial) editor.UpdateInitialDialog(createdNode.DialogData); 
        });//create a new node, and if its the only in the tree, then update the initialDialog of the tree to its
    }

    List<bool> initializedConnections = new List<bool>();
    public void PopulateView(DialogTree prevTree, DialogTree newTree)
    {
        isPopulating = true;
        if(prevTree != null)
        {
            foreach (var nodeView in nodeViews)
            {
                if (nodeView.node.IsInitial)
                    continue;

                if (!nodeView.input.connected)
                    prevTree.UnusedNodes.Add(nodeView.node.DialogData);
            }

        }

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        nodeViews.Clear();

        if (newTree == null)
            return;

        DialogData prevDialog = newTree.InitialDialog;
        if (prevDialog == null)
            return;

        DialogNodeView prevNodeView = CreateNode(prevDialog).View;
        List<DialogNodeView> pendingDialogs = new List<DialogNodeView>();//this list holds already generated nodes that still have connections to be generated

        bool generatingNodes = true;
        while (generatingNodes)
        {
            foreach (DialogData nextDialog in prevDialog.PossibleNextDialogs.Keys)//this cycle generates every node connected to prevDialog and connects them
            {
                DialogNodeView nextNodeView = CreateNode(nextDialog).View;
                CreateEdge(prevNodeView, nextNodeView, prevDialog.PossibleNextDialogs[nextDialog]);
                initializedConnections.Add(false);

                if (nextDialog.PossibleNextDialogs.Count == 0)//if the generated node doesn't have any output nodes, then we won't be interested on it anymore
                    continue;

                pendingDialogs.Add(nextNodeView);
            }

            if (pendingDialogs.Count == 0)
            {
                if(newTree.UnusedNodes.Count > 0)
                {
                    prevDialog = newTree.UnusedNodes[0];
                    prevNodeView = CreateNode(prevDialog).View;

                    newTree.UnusedNodes.Remove(prevDialog);
                    
                    continue;
                }
                generatingNodes = false;
                continue;
            }

            prevNodeView = pendingDialogs[0];
            prevDialog = prevNodeView.node.DialogData;

            pendingDialogs.Remove(pendingDialogs[0]);

        }
        while (!AllConnectionsInitialized()) { }
        Debug.Log("isPopulating from GraphView: " + isPopulating);
        isPopulating = false;   
    }
    public void ConnectionInitialized()
    {
        for(int i = 0; i < initializedConnections.Count(); i++)
        {
            if (!initializedConnections[i])
            {
                initializedConnections[i] = true;
                return;
            }
        }
    }
    bool AllConnectionsInitialized()
    {
        foreach (bool connection in initializedConnections)
            if (!connection) return false;
        return true;
    }

    GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
            foreach (var elem in graphViewChange.elementsToRemove)
            {
                DialogNodeView nodeView = elem as DialogNodeView;
                if (nodeView != null)
                {
                    nodeView.RemoveFromHierarchy();
                    editor.RemoveNode(nodeView.node);

                    nodeViews.Remove(nodeView);
                    if (!nodeView.node.IsInitial)
                        continue;

                    editor.UpdateInitialDialog(nodeViews.Count > 0 ? nodeViews[0].node.DialogData : null);

                    if (nodeViews.Count > 0)
                    {
                        nodeViews[0].inputContainer.Remove(nodeViews[0].input);
                        nodeViews[0].node.IsInitial = true;
                    }
                    continue;
                }

                DialogEdge dialogEdge = elem as DialogEdge;
                if (dialogEdge != null)
                {
                    dialogEdge.RemoveFromHierarchy();
                    dialogEdge.OnRemoved();
                    Debug.Log("Removed edge");
                    continue;
                }
            }
        if (graphViewChange.edgesToCreate != null)
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                DialogEdge dialogEdge = edge as DialogEdge;
                if (dialogEdge != null)
                {
                    dialogEdge.Start(editor.inspectorView, this);
                }
            });

        return graphViewChange;
    }

    DialogNode CreateNode(DialogData value)
    {
        if (!editor.IsTreeSet())
        {
            Debug.LogError("Tree must not be null");
            return null;
        }

        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.DialogData = value;
        node.IsInitial = nodeViews.Count == 0 ? true : false;

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
        DialogEdge dialogEdge = outputNode.output.ConnectTo<DialogEdge>(inputNode.input);
        AddElement(dialogEdge);

        dialogEdge.Start(editor.inspectorView, this);
        dialogEdge.dialogConnection.ConnectionChangeConditions = dialogChangeConditions;
        Debug.Log(dialogEdge.dialogConnection.ConnectionChangeConditions.Length);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endport => endport.direction != startPort.direction && endport.node != startPort.node).ToList();
    }
}