using System;
using UnityEditor;

public class TreeEditorWindow : EditorWindow
{
    // -- METHODS
    
    
    [MenuItem("Tools/TreeEditor")]
    static void Init()
    {
        TreeEditorWindow window = (TreeEditorWindow)EditorWindow.GetWindow(typeof(TreeEditorWindow));
        window.Show();
    }
    
    // -- UNITY

    private void OnGUI()
    {
        
    }
}
