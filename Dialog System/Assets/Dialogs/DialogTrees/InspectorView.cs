using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    public InspectorView() { }

    public Object editingObject;

    public void UpdateSelection(Object editingObject) 
    {
        Editor editor = Editor.CreateEditor(editingObject);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        this.editingObject = editingObject;
    }
}
