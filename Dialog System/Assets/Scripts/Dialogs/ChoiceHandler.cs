using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandler
{
    DialogTreeDriver dialogTreeDriver;
    ChoiceButtonManager choiceButtonManager;

    public ChoiceHandler(DialogTreeDriver dialogTreeDriver)
    { 
        this.dialogTreeDriver = dialogTreeDriver;
        choiceButtonManager = this.dialogTreeDriver.choiceButtonManager;
    }

    public void DisplayChoiceButtons(ChoiceOption[] options)
    {
        choiceButtonManager.GenerateButtons(options);
    }

    public void OnOptionSelected(int selectedBranchIndex)
    {
        dialogTreeDriver.currDialogDriver.OnBranchChanged(selectedBranchIndex);
    }
}
