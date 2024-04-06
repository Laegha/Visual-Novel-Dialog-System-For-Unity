using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
    Dialog[] dialogs;
    DialogNodeView[] nodeViews = new DialogNodeView[0];

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
        if (currTree != treeChangeView.TreeChange.currentlyEditingTree)
        {
            currTree = treeChangeView.TreeChange.currentlyEditingTree;
            //Debug.Log("Tree Update");
            treeView.PopulateView(currTree);
        }

        if (nodeViews.Length != treeView.nodeViews.Count)
        {
            nodeViews = treeView.nodeViews.ToArray();
            for (int i = 0; i < nodeViews.Length; i++)
            {
                if (nodeViews[i] == null)
                {
                    treeView.nodeViews.Remove(nodeViews[i]);
                }
            }
        }

        foreach (DialogNodeView dialogNodeView in treeView.nodeViews)
            if (dialogNodeView == null)
                treeView.nodeViews.Remove(dialogNodeView);
        //treeView.nodeViews.Clear();

        List<Dialog> dialogDynamicList = new List<Dialog>();
        treeView.nodeViews.ForEach(node => dialogDynamicList.Add(node.node.Dialog));

        if(dialogs != dialogDynamicList.ToArray())
        {
            //Debug.Log("Dialog Update");
            //Debug.Log("Dialog Update");
            dialogs = dialogDynamicList.ToArray();
            currTree.Dialogs = dialogs;
        }
        //dialogs = recorrer una lista con los DialogNodeView.Node.Dialog
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
