using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeChangeView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<TreeChangeView, VisualElement.UxmlTraits> { }

    //TreeChange treeChange = ScriptableObject.CreateInstance("TreeChange") as TreeChange;
    TreeChange treeChange = new TreeChange();

    public TreeChange TreeChange { get { return treeChange; } }

    public TreeChangeView()
    {
        Editor editor = Editor.CreateEditor(treeChange);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI();});
        Add(container);

    }
}
