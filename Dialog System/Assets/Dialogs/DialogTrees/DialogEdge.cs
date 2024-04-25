using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    public InspectorView inspectorView;

    public DialogConnection dialogConnection = new DialogConnection();

    public override void OnSelected()
    {
        base.OnSelected();

        Debug.Log("Edge succesfully selected");

        inspectorView.UpdateSelection(dialogConnection);
    }

    public void UpdateValues()
    {
        DialogNodeView nodeView = input.node as DialogNodeView;
        //nodeView.node.Dialog.possibleNextDialogs
    }
}