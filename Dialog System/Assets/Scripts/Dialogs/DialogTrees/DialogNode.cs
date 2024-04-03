using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNode : ScriptableObject
{
    [SerializeField] Dialog dialog;

    public Dialog Dialog { set { dialog = value; } get{ return dialog; } }

    //in condition
    //out conditions[]
}
