using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class DialogData
{
    [SerializeField] Dialog _dialog;
    string _branch;
    Vector2 _nodePosition;
    bool _isPlayable;
    Dictionary<DialogData, List<DialogChangeCondition>> _possibleNextDialogs;

    public Dialog Dialog {  get { return _dialog; } set { _dialog = value; } }
    public string Branch {  get { return _branch; } set { _branch = value; } }
    public Vector2 NodePosition {  get { return _nodePosition; } set { _nodePosition = value; } }
    public bool IsPlayable{  get { return _isPlayable; } set { _isPlayable = value; } }
    public Dictionary<DialogData, List<DialogChangeCondition>> PossibleNextDialogs { get { return _possibleNextDialogs; } set { _possibleNextDialogs = value; } }

    public DialogData(Dialog dialog, string branch)
    {
        _dialog = dialog;
        _branch = branch;
        _possibleNextDialogs = new Dictionary<DialogData, List<DialogChangeCondition>>();
    }
}
