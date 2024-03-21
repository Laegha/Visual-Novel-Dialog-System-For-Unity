using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogTreeView : GraphView
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    public new class UxmlFactory : UxmlFactory<DialogTreeView, UxmlTraits> { }

    public DialogTreeView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var labelFromUXML = m_VisualTreeAsset.Instantiate();
        Add(labelFromUXML);
    }
}
