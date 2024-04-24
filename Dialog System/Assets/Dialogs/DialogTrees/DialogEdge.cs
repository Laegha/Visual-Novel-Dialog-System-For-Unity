using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    [SerializeField] DialogChangeCondition[] dialogChangeConditions;
    InspectorView inspectorView;
    public override void OnSelected()
    {
        base.OnSelected();

        Debug.Log("Edge succesfully selected");
        
        
    }
}
