using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    public InspectorView() { }

    //public Object editingObject;
    //IMGUIContainer currContainer;
    public Dictionary<Object, IMGUIContainer> editingObjectsContainers = new Dictionary<Object, IMGUIContainer>();

    public void UpdateSelection(Object editingObject) 
    {
        Editor editor = Editor.CreateEditor(editingObject);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        editingObjectsContainers.Add(editingObject, container);
        //currContainer = container;
        //this.editingObject = editingObject;
    }

    public void RemoveCurrentSelection(Object removedObject)
    {
        Remove(editingObjectsContainers[removedObject]);
        editingObjectsContainers.Remove(removedObject);
        //editingObject = null;
    }
}
