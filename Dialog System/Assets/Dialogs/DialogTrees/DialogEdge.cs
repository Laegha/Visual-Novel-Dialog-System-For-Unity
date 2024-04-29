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

        dialogConnection.outputNode = outputNode.node;
        dialogConnection.inputNode = inputNode.node;
        dialogConnection.prevOutputDialog = outputNode.node.Dialog;
        dialogConnection.prevInputDialog = inputNode.node.Dialog;

        outputNode.node.OutputConnections.Add(dialogConnection);
        inputNode.node.InputConnections.Add(dialogConnection);

        dialogConnection.UpdateDialogs();
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

    public void OnRemoved()
    {

    }
}
