using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogEdge : Edge
{
    InspectorView inspectorView;

    public DialogConnection dialogConnection = ScriptableObject.CreateInstance("DialogConnection") as DialogConnection;

    public void Start(InspectorView inspectorView)
    {
        this.inspectorView = inspectorView;
        DialogNodeView outputNode = output.node as DialogNodeView;
        DialogNodeView inputNode = input.node as DialogNodeView;

        dialogConnection.connectionName = outputNode.title + "->" + inputNode.title;

        dialogConnection.outputNode = outputNode.node;
        dialogConnection.inputNode = inputNode.node;
        dialogConnection.prevOutputDialog = outputNode.node.DialogData;
        dialogConnection.prevInputDialog = inputNode.node.DialogData;

        outputNode.node.OutputConnections.Add(dialogConnection);
        inputNode.node.InputConnection = dialogConnection;

        inputNode.node.DialogData.Branch = outputNode.node.DialogData.Branch + ":" + outputNode.output.connections.Count();

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
        (input.node as DialogNodeView).node.DialogData.Branch = "";
        dialogConnection.UnlinkConnection();
    }
}
