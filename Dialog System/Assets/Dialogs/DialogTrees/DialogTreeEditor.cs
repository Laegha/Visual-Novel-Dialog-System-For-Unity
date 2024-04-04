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
        treeChangeView.dialogTreeEditor = this;
    }

    public void OnTreeChange(DialogTree newTree)
    {
        currTree = newTree;
        if (currTree == null)
            return;

        Debug.Log("Cambio de seleccion a " + Selection.activeObject);
        
    }

    public bool IsTreeRefreshed()
    {
        if (currTree != treeChangeView.TreeChange.currentlyEditingTree)
        {
            Debug.LogError("Tree must be refreshed before making any changes", this);
            return false;
        }

        if(currTree == null)
        {
            Debug.LogError("Not editing any tree", this);
            return false;
        }

        return true;
    }

    void OnNodeSelected(DialogNodeView node)
    {
        if (!IsTreeRefreshed())
            return;
        inspectorView.UpdateSelection(node);
    }
    //void OnNodeSelectionChanged(DialogNodeView nodeView)
    //{
    //    inspectorView.UpdateSelection
    //}
}
