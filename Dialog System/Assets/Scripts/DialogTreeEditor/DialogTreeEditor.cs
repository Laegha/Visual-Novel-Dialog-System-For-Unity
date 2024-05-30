using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    DialogTreeGraphView treeView;

    public InspectorView inspectorView;
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
            if(dialogNode.Dialog != dialogNode.PrevDialog)
            {
                ChangeNodeDialog(dialogNode);

            }
        }

        foreach(Object editingObject in inspectorView.editingObjectsContainers.Keys)
        {
            DialogConnection dialogConnection = editingObject as DialogConnection;
            if(dialogConnection != null)
            {
                if (!dialogConnection.isConnectionPossible)
                    continue;

                dialogConnection.Update();
            }
        }
    }

    public bool IsTreeSet()
    {
        return currTree != null;
    }

    public void AddNode(DialogNode newNode)
    {
        dialogNodes.Add(newNode);
    }

    public void RemoveNode(DialogNode removedNode)
    {
        dialogNodes.Remove(removedNode);
        ReloadNodeIndexes(removedNode.DialogIndex);
    }

    void ReloadNodeIndexes(int removedIndex)
    {
        for (int i = removedIndex; i < dialogNodes.Count; i++)
        {
            dialogNodes[i].DialogIndex = i;
            //call a function on dialogNodes[i].View to handle ports
        }
    }

    void ChangeNodeDialog(DialogNode newNode)
    {
        newNode.PrevDialog = newNode.Dialog;

        newNode.View.title = newNode.Dialog != null ? newNode.Dialog.name : "New Dialog";

        newNode.InputConnections.ForEach(connection => connection.UpdateDialogs()); 
        newNode.OutputConnections.ForEach(connection => connection.UpdateDialogs());
    }
}
