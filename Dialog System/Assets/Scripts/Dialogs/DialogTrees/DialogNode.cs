using UnityEngine;
using UnityEngine.UIElements;

public class DialogNode : ScriptableObject
{
    [SerializeField] Dialog dialog;

    public Dialog Dialog { set { dialog = value; } get{ return dialog; } }

    public void RegisterDialogCallback(DialogNodeView view) 
    { 
        view.RegisterCallback<ChangeEvent<DialogNode>>(view.OnDialogChange);
    }

    //in condition
    //out conditions[]
}
