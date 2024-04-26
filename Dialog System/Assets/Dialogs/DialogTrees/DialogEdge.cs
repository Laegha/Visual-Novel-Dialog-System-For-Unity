using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    public InspectorView inspectorView;

    public DialogConnection dialogConnection = ScriptableObject.CreateInstance("DialogConnection") as DialogConnection;

    public override void OnSelected()
    {
        base.OnSelected();

        inspectorView.UpdateSelection(dialogConnection);
    }
    public override void OnUnselected()
    {
        base.OnUnselected();


    }
    public void UpdateValues()
    {
        DialogNodeView nodeView = input.node as DialogNodeView;
        //nodeView.node.Dialog.possibleNextDialogs
    }
}