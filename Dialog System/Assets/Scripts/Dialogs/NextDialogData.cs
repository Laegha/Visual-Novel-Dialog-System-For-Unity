using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDialogData
{
    Dialog _dialog;
    int _neededProgression;

    public Dialog Dialog {  get { return _dialog; } set { _dialog = value; } }
    public int NeededProgression {  get { return _neededProgression; } set { _neededProgression = value; } }
}
