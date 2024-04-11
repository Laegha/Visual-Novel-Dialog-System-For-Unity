using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "ScriptableObjects/Dialogs/DialogTree", order = 1)]
public class DialogTree : ScriptableObject
{
    public SerializedDictionary<string, Dialog> dialogs = new SerializedDictionary<string, Dialog>();
    

    public SerializedDictionary<string, Dialog> Dialogs { set{ dialogs = value; } get { return dialogs; } }
}
