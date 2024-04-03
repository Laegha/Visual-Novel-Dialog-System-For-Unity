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
    public DialogNodeView()
    {
        Editor editor = Editor.CreateEditor(node != null ? node : ScriptableObject.CreateInstance("DialogNode") as DialogNode);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        if (node == null)
            return;
        title = node.Dialog.name;
    }


    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }
}
