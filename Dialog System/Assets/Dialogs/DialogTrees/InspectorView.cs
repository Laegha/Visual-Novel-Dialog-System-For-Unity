using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    public InspectorView() { }

    public Object editingObject;
    IMGUIContainer currContainer;

    public void UpdateSelection(Object editingObject) 
    {
        Editor editor = Editor.CreateEditor(editingObject);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
        Remove(container);
        currContainer = container;
        this.editingObject = editingObject;
    }

    public void RemoveCurrentSelection()
    {
        Remove(currContainer);
        editingObject = null;
    }
}
