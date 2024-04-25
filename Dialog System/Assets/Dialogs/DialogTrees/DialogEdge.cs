using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    [SerializeField] DialogChangeCondition[] dialogChangeConditions;
    public InspectorView inspectorView;
    public override void OnSelected()
    {
        base.OnSelected();

        Debug.Log("Edge succesfully selected");

        inspectorView.UpdateSelection(this);
    }

    public void UpdateValues()
    {
        DialogNodeView nodeView = input.node as DialogNodeView;
        //nodeView.node.Dialog.possibleNextDialogs
    }
}