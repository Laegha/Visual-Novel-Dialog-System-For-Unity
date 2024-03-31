using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TreeChangeView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<TreeChangeView, VisualElement.UxmlTraits> { }

    TreeChange treeChange = ScriptableObject.CreateInstance("TreeChange") as TreeChange;

    Action refreshNodes;

    public TreeChangeView()
    {
        Editor editor = Editor.CreateEditor(treeChange);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI();});
        Add(container);

        refreshNodes += RefreshNodes;
        Button refreshButton = new Button();
        refreshButton.text = "Refresh Graph";
        refreshButton.clicked += refreshNodes;
        Add(refreshButton);
    }

    void RefreshNodes()
    {

    }
}
