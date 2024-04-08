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
    Dictionary<DialogNode, Dialog> dialogs = new Dictionary<DialogNode, Dialog>();

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
            treeView.PopulateView(currTree);
        }

        foreach(DialogNode dialogNode in dialogs.Keys) 
        {
            if(dialogNode.Dialog != dialogs[dialogNode])
                ChangeNodeDialog(dialogNode);
        }
    }

    public bool IsTreeRefreshed()
    {
        return currTree != null;
    }

    void OnNodeSelected(DialogNodeView node)
    {
        inspectorView.UpdateSelection(node);
    }

    public void AddNode(DialogNode newNode)
    {
        dialogs.Add(newNode, newNode.Dialog);
        currTree.Dialogs = GetDialogArray();
    }

    public void RemoveNode(DialogNode newNode)
    {
        dialogs.Remove(newNode);
        currTree.Dialogs = GetDialogArray();
    }

    void ChangeNodeDialog(DialogNode newNode)
    {
        dialogs[newNode] = newNode.Dialog;
        currTree.Dialogs = GetDialogArray();
    }

    Dialog[] GetDialogArray()
    {
        Dialog[] dialogArray = new Dialog[dialogs.Count];
        int i = 0;
        foreach(KeyValuePair<DialogNode, Dialog> keyValuePair in dialogs)
        {
            dialogArray[i] = keyValuePair.Value;
            Debug.Log(keyValuePair.Value);
            i++;
        }
        return dialogArray;
    }
}
