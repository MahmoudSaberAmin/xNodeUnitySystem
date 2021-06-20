using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(GraphController))]
public class GraphControllerEditor : Editor
{

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelected;
    }

    private void OnDisable()
    {
        
        Selection.selectionChanged -= OnSelected;
    }

    private void OnSelected()
    {
        XNodeEditor.NodeEditorWindow.Open((target as GraphController).Graph);
    }
}
