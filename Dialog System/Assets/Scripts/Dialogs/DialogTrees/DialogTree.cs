using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogTree", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    Dictionary<string, Dialog> dialogs = new Dictionary<string, Dialog>();
    
    Dictionary<string, Vector2> nodePositions = new Dictionary<string, Vector2>();

    public Dictionary<string, Dialog> Dialogs { set{ dialogs = value; } get { return dialogs; } }
    public Dictionary<string, Vector2> NodePositions { set{ nodePositions = value; } get { return nodePositions; } }

}
