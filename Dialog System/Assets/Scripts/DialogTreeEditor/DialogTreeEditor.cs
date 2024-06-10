using System.Collections.Generic;
using System.Linq;
using TreeEditor;
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
        treeView.editor = this;
        inspectorView = root.Q<InspectorView>();
        treeChangeView = root.Q<TreeChangeView>();

    }

    private void OnGUI()
    {
        if (currTree != treeChangeView.TreeChange.currentlyEditingTree)
        {
            DialogTree prevTree = currTree;
            currTree = treeChangeView.TreeChange.currentlyEditingTree;
            dialogNodes.Clear();
            treeView.PopulateView(prevTree, currTree);
        }

        foreach(DialogNode dialogNode in dialogNodes)
        {
            if(dialogNode.DialogData.Dialog != dialogNode.PrevDialog)
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


    void ChangeNodeDialog(DialogNode newNode)
    {
        newNode.PrevDialog = newNode.DialogData.Dialog;

        newNode.View.title = newNode.DialogData.Dialog != null ? newNode.DialogData.Dialog.name : "New Dialog";

        if(newNode.InputConnection != null)
            newNode.InputConnection.UpdateDialogs(); 

        newNode.OutputConnections.ForEach(connection => connection.UpdateDialogs());

        if (newNode.IsInitial)
            UpdateInitialDialog(newNode.DialogData);
    }
    public void AddNode(DialogNode newNode) => dialogNodes.Add(newNode);

    public void RemoveNode(DialogNode removedNode) => dialogNodes.Remove(removedNode);

    public void UpdateInitialDialog(DialogData newInitialDialog) => currTree.InitialDialog = newInitialDialog;
}
