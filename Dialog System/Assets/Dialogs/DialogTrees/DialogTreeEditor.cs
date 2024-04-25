using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
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

    public DialogTree currTree = null;
    List<DialogNode> dialogNodes = new List<DialogNode>();

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
        //treeView.OnNodeSelected = OnNodeSelected;
        treeView.editor = this;
        inspectorView = root.Q<InspectorView>();
        treeChangeView = root.Q<TreeChangeView>();
    }

    private void OnGUI()
    {
        if (currTree != treeChangeView.TreeChange.currentlyEditingTree)
        {
            currTree = treeChangeView.TreeChange.currentlyEditingTree;
            //dialogs = currTree.Dialogs;
            dialogNodes.Clear();
            treeView.PopulateView(currTree);
        }

        foreach(DialogNode dialogNode in dialogNodes)
        {
            if(dialogNode.Dialog != currTree.Dialogs[dialogNode.DialogIndex])
            {
                ChangeNodeDialog(dialogNode);
            }
        }
    }

    public bool IsTreeRefreshed()
    {
        return currTree != null;
    }

    public void AddNode(DialogNode newNode)
    {
        dialogNodes.Add(newNode);

        if (currTree.Dialogs.ContainsKey(newNode.DialogIndex))
            return;

        currTree.Dialogs.Add(newNode.DialogIndex, newNode.Dialog);
        currTree.NodePositions.Add(newNode.DialogIndex, Vector2.zero);
    }

    public void RemoveNode(DialogNode newNode)
    {
        dialogNodes.Remove(newNode);
        currTree.Dialogs.Remove(newNode.DialogIndex);
        currTree.NodePositions.Remove(newNode.DialogIndex);
    }

    void ChangeNodeDialog(DialogNode newNode)
    {
        currTree.Dialogs[newNode.DialogIndex] = newNode.Dialog;
        newNode.View.title = newNode.Dialog != null ? newNode.Dialog.name : "New Dialog";
    }

    public VisualElement FindElementInHierarchy(string type)
    {
        return rootVisualElement.Q(type);
    }
}
