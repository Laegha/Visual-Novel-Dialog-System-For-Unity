using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler
{
    DialogDriver thisDialogDriver;

    public ChoiceHandler(DialogDriver dialogDriver)
    { 
        thisDialogDriver = dialogDriver;
    }

    public void DisplayChoiceButtons(ChoiceOption[] options)
    {
        
    }

    void OnOptionSelected(int selectedBranchIndex)
    {
        thisDialogDriver.OnBranchChanged(selectedBranchIndex);
    }
}
