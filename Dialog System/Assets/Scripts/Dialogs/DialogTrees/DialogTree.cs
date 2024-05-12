using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogTree", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    List<Dialog> dialogs = new List<Dialog>();
    
    List<Vector2> nodePositions = new List<Vector2>();

    public List<Dialog> Dialogs { set{ dialogs = value; } get { return dialogs; } }
    public List<Vector2> NodePositions { set{ nodePositions = value; } get { return nodePositions; } }

}
