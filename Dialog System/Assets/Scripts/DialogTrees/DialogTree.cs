using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogTree", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    DialogData initialDialog = null;
    List<DialogData> unusedNodes = new List<DialogData>();

    public DialogData InitialDialog { get { return initialDialog; } set { initialDialog = value; } }
    public List<DialogData> UnusedNodes { get { return unusedNodes; } set { unusedNodes = value; } }
    //probably add things related to the main character AKA protagonist
}
