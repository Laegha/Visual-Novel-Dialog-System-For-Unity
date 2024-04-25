using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : Object
{
    [SerializeField] Dialog dialog;
    string dialogIndex;
    DialogNodeView dialogNodeView;

    public Dialog Dialog { set { dialog = value; } get{ return dialog; } }
    public string DialogIndex{ set { dialogIndex = value; } get{ return dialogIndex; } }
    public DialogNodeView View{ set { dialogNodeView = value; } get{ return dialogNodeView; } }

    //in condition
    //out conditions[]
}
