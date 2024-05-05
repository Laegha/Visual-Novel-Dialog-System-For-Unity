using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler
{
    DialogTreeDriver dialogTreeDriver;

    public ChoiceHandler(DialogTreeDriver dialogDriver)
    { 
        dialogTreeDriver = dialogDriver;
    }

    public void DisplayChoiceButtons(ChoiceOption[] options)
    {
        
    }

    void OnOptionSelected(int selectedBranchIndex)
    {
        dialogTreeDriver.currDialogDriver.OnBranchChanged(selectedBranchIndex);
    }
}
