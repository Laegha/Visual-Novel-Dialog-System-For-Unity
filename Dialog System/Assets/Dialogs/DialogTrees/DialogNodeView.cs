using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogNodeView : Node
{
    public new class UxmlFactory : UxmlFactory<DialogNodeView, Node.UxmlTraits> { }

    public DialogNode node;
    
    public DialogNodeView()
    {
    }


    public override void OnSelected()
    {

    }
}
