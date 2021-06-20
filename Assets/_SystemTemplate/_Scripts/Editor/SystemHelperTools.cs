using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;


[ExecuteInEditMode]
public class SystemHelperTools : EditorWindow
{
    string _fileName = "File Name";
    private void OnGUI()
    {
        _fileName = GUILayout.TextField(_fileName, 60);

        
        //if(GUILayout.Button("Save Graph"))
        //{
        //    SaveGraph(_fileName);
        //}       
        
        //if(GUILayout.Button("Load Graph"))
        //{
        //    LoadGraph(_fileName);
        //}  
        
        if(GUILayout.Button("Move"))
        {
            Selection.selectionChanged += Moved;
            Move();

        }       
    }

    private void Moved()
    {
        Selection.selectionChanged -= Moved;
        foreach (var o in _objectsToMove)
        {
            Undo.SetTransformParent(o.transform, Selection.activeTransform, "Arabtesting Move");
        }
    }
    private GameObject[] _objectsToMove;

    private void Move()
    {
        _objectsToMove = Selection.gameObjects;
    }


    [MenuItem("Tools/System Helper")]
    private static void OpenWindow()
    {
        GetWindow<SystemHelperTools>(false, "System Helper", true);
    }



    //public void SaveData()
    //{
    //    NewSystemSerializer.SaveNode(_situations, "C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\data.json");
    //}

    //public void LoadData()
    //{
    //    _situations1 = NewSystemSerializer.LoadNode("C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\data.json", _situations.Count);
    //}


    public static void SaveGraph(string fileName)
    {
        NewSystemSerializer.SaveGraph(Resources.Load<SystemsGraph>(fileName), "C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\"+ fileName);

        Logger.Log("Success!");
    }

    public static void LoadGraph(string fileName)
    {
        var graph = NewSystemSerializer.LoadGraph("C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\"+ fileName);

        AssetDatabase.CreateAsset(graph, "Assets\\_SystemTemplate\\Graphs\\Restored\\"+ fileName+".asset");//C:\\NewSystemStructure\\NewSystemStructureProject\\
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var so = AssetDatabase.LoadAssetAtPath<NodeGraph>("Assets\\_SystemTemplate\\Graphs\\Restored\\" + fileName + ".asset");//graph.Copy(); // ScriptableObject.CreateInstance<SystemsGraph>();

        foreach (var node in graph.nodes)
        {
            AssetDatabase.AddObjectToAsset(node, so);
            AssetDatabase.SaveAssets();
            NodeEditorWindow.RepaintAll();
        }

        AssetDatabase.Refresh();


        Logger.Log("Load Done!!");
    }

}