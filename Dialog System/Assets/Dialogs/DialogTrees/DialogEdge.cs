using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    InspectorView inspectorView;

    DialogConnection dialogConnection = ScriptableObject.CreateInstance("DialogConnection") as DialogConnection;

    public void Start(InspectorView inspectorView)
    {
        this.inspectorView = inspectorView;
        DialogNodeView outputNode = output.node as DialogNodeView;
        DialogNodeView inputNode = input.node as DialogNodeView;

        dialogConnection.connectionName = outputNode.title + "->" + inputNode.title;

        dialogConnection.outputDialog = outputNode.node;
        dialogConnection.inputDialog = inputNode.node;
    }

    public override void OnSelected()
    {
        base.OnSelected();

        inspectorView.UpdateSelection(dialogConnection);
    }

    public override void OnUnselected()
    {
        base.OnUnselected();

        inspectorView.RemoveCurrentSelection(dialogConnection);
    }

    public void UpdateValues()
    {
        DialogNodeView nodeView = input.node as DialogNodeView;
        //nodeView.node.Dialog.possibleNextDialogs
    }

    public void DialogChanged()
    {
        if(dialogConnection.outputDialog.Dialog == dialogConnection.inputDialog.Dialog)

    }

    public void OnRemoved()
    {

    }
}
