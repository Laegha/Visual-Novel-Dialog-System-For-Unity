using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    public Dictionary<string, Dialog> dialogs = new Dictionary<string, Dialog>();
    

    public Dictionary<string, Dialog> Dialogs { set{ dialogs = value; } get { return dialogs; } }
}
