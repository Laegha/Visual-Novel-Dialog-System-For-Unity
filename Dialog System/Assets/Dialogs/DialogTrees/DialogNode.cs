using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine.UIElements;

public class DialogNode : Node
{
    public new class UxmlFactory : UxmlFactory<DialogNode, Node.UxmlTraits> { }

    public DialogNode()
    {
        TextField text = new TextField();
        ObjectField field = new ObjectField();
    }
}
