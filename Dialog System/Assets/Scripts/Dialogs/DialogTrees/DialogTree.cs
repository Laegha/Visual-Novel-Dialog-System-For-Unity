using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    Dialog[] dialogs;

    public Dialog[] Dialogs { set{ dialogs = value; } get { return dialogs; } }
}
