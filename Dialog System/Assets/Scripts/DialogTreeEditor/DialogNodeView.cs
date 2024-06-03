using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogNodeView : Node
{
    public new class UxmlFactory : UxmlFactory<DialogNodeView, Node.UxmlTraits> { }

    public DialogNode node;

    public DialogTreeEditor treeEditor;

    public Port input;
    public Port output;

    public void Start()
    {
        node.View = this;

        Editor editor = Editor.CreateEditor(node);
        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);

        if(!node.IsInitial)
            CreateInput();
        
        CreateOutput();

        if (node.Dialog == null)
            return;

        title = node.Dialog.name;

    }

    void CreateInput()
    {
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(DialogChangeCondition));
        input.portName = "";
        inputContainer.Add(input);
    }

    void CreateOutput()
    {
        output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(DialogChangeCondition));
        output.portName = "";
        outputContainer.Add(output);
        
    }

    public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
    {
        return Port.Create<DialogEdge>(orientation, direction, capacity, type);
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Vector2 newPosVector = new Vector2(newPos.xMin, newPos.yMin);
    }
}
