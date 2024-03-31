using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;

public class DialogTreeGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeGraphView, GraphView.UxmlTraits> { }
    public DialogTreeGraphView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Dialogs/DialogTrees/DialogTreeEditor.uss");
        styleSheets.Add(styleSheet);

    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        evt.menu.AppendAction($"[{System.Type.GetType("DialogNode")}]", (a) => CreateNode());
    }

    void CreateNode()
    {
        DialogNode node = new DialogNode();
        CreateNodeView(node);
    }

    void CreateNodeView(DialogNode node)
    {
        DialogNodeView newNode = new DialogNodeView();
        newNode.node = node;
        newNode.title = "New DialogNode";
        AddElement(newNode);
    }

}
