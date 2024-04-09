using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using System.Collections.Generic;

public class DialogTreeGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeGraphView, GraphView.UxmlTraits> { }

    public Action<DialogNodeView> OnNodeSelected;

    public DialogTreeEditor editor;

    List<DialogNodeView> nodeViews = new List<DialogNodeView>();

    public DialogTreeGraphView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Dialogs/DialogTrees/DialogTreeEditor.uss");
        styleSheets.Add(styleSheet);

    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction($"[{Type.GetType("DialogNode")}]", (a) => CreateNode(null));
    }

    public void PopulateView(DialogTree dialogTree)
    {
        if (nodeViews.Count > 0)
            foreach (DialogNodeView nodeView in nodeViews)
                nodeView.RemoveFromHierarchy();
        if (dialogTree == null)
            return; 
        //dialogTree.Dialogs = new Dialog[0];
        foreach (Dialog dialog in dialogTree.Dialogs)
            CreateNode(dialog);
    }
    
    void CreateNode(Dialog nodeDialog)
    {
        if (!editor.IsTreeRefreshed())
            return;
        //DialogNode node = new DialogNode(nodeDialog);
        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.Dialog = nodeDialog;
        CreateNodeView(node);
    }

    void CreateNodeView(DialogNode node)
    {
        DialogNodeView newNodeView = new DialogNodeView();
        newNodeView.node = node;
        newNodeView.editor = editor;
        newNodeView.OnNodeSelected = OnNodeSelected;
        newNodeView.title = "New DialogNode";
        AddElement(newNodeView);
        editor.AddNode(newNodeView.node);
        nodeViews.Add(newNodeView);
    }
}