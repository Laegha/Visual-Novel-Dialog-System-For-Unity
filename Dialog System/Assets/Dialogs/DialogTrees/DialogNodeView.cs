using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNodeView : Node
{
    public new class UxmlFactory : UxmlFactory<DialogNodeView, Node.UxmlTraits> { }

    public DialogNode node;
    public Action<DialogNodeView> OnNodeSelected;

    public DialogTreeEditor treeEditor;

    public void Start()
    {
        node.View = this;

        Editor editor = Editor.CreateEditor(node);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        Button removeNodeBtn = new Button();
        removeNodeBtn.text = "Remove Node";
        removeNodeBtn.clicked += RemoveNode;
        Add(removeNodeBtn);

        style.left = treeEditor.currTree.NodePositions[node.DialogIndex].x;
        style.top = treeEditor.currTree.NodePositions[node.DialogIndex].y;

        if (node.Dialog == null)
            return;

        title = node.Dialog.name;

    }
    void RemoveNode()
    {
        parent.hierarchy.Remove(this);
        treeEditor.RemoveNode(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Vector2 newPosVector = new Vector2(newPos.xMin, newPos.yMin);
        treeEditor.currTree.NodePositions[node.DialogIndex] = newPosVector;
    }
}
