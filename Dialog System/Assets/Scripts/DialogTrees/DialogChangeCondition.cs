using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogChangeCondition", menuName = "ScriptableObjects/Dialogs/DialogChangeCondition", order = 1)]
public class DialogChangeCondition : ScriptableObject
{
    public virtual bool IsFullfilled()
    {
        return true;
    }
}
