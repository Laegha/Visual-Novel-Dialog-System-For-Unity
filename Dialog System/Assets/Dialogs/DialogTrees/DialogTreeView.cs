using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

public class DialogTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeView, GraphView.UxmlTraits> { }
    public DialogTreeView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Dialogs/DialogTrees/DialogTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
