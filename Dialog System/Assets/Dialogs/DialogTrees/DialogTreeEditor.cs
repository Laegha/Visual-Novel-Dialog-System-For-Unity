using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    DialogTreeGraphView treeView;
    InspectorView inspectorView;
    TreeChangeView treeChangeView;

    DialogTree currTree;

    [MenuItem("Window/DialogTreeEditor")]
    public static void ShowExample()
    {
        DialogTreeEditor wnd = GetWindow<DialogTreeEditor>();
        wnd.titleContent = new GUIContent("DialogTreeEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        treeView = root.Q<DialogTreeGraphView>();
        treeView.OnNodeSelected = OnNodeSelected;
        treeView.editor = this;
        inspectorView = root.Q<InspectorView>();
        treeChangeView = root.Q<TreeChangeView>();
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        currTree = treeChangeView.TreeChange.currentlyEditingTree;
        if (EditorGUI.EndChangeCheck())
            treeView.PopulateView(currTree);
    }

    public bool IsTreeRefreshed()
    {
        return currTree != null;
    }

    void OnNodeSelected(DialogNodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    //void OnNodeDialogChange()
    //{
    //    if(!IsTreeRefreshed())
    //        return;
    //    currTree
    //}
}
