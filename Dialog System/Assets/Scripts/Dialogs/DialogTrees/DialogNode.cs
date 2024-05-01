using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : ScriptableObject
{
    [SerializeField] Dialog dialog;
    string dialogIndex;
    DialogNodeView dialogNodeView;
    List<DialogConnection> inputConnections = new List<DialogConnection>();
    List<DialogConnection> outputConnections = new List<DialogConnection>();

    public Dialog Dialog { set { dialog = value; } get{ return dialog; } }
    public string DialogIndex{ set { dialogIndex = value; } get{ return dialogIndex; } }
    public DialogNodeView View{ set { dialogNodeView = value; } get{ return dialogNodeView; } }
    public List<DialogConnection> InputConnections { set { inputConnections = value; } get{ return inputConnections; } }
    public List<DialogConnection> OutputConnections { set { outputConnections = value; } get{ return outputConnections; } }

    //in condition
    //out conditions[]
}
