using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    public InspectorView() { }

    public void UpdateSelection(UnityEngine.Object editingObject) 
    {
        Editor editor = Editor.CreateEditor(editingObject);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
    }
}
