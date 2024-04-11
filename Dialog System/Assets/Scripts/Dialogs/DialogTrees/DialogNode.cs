using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : ScriptableObject
{
    [SerializeField] Dialog dialog;
    string dialogIndex;

    public Dialog Dialog { set { dialog = value; } get{ return dialog; } }
    public string DialogIndex{ set { dialogIndex = value; } get{ return dialogIndex; } }

    //in condition
    //out conditions[]
}
