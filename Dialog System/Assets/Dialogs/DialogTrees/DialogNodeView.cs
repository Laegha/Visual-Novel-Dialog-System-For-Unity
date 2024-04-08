using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNodeView : Node
{
    public new class UxmlFactory : UxmlFactory<DialogNodeView, Node.UxmlTraits> { }

    public DialogNode node;
    public Action<DialogNodeView> OnNodeSelected;

    public DialogTreeEditor editor;

    public DialogNodeView()
    {
        DialogNode createdNode = node != null ? node : ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        createdNode.RegisterDialogCallback(this);
        Editor editor = Editor.CreateEditor(node != null ? node : ScriptableObject.CreateInstance("DialogNode") as DialogNode);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        Button removeNodeBtn = new Button();
        removeNodeBtn.text = "Remove Node";
        removeNodeBtn.clicked += RemoveNode;
        Add(removeNodeBtn);

        if (node == null)
            return;
        title = node.Dialog.name;
        
    }

    void RemoveNode()
    {
        parent.hierarchy.Remove(this);
        editor.RemoveNode(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }

    public void OnDialogChange(ChangeEvent<DialogNode> evt)
    {
        Debug.Log(evt);
    }
}
