using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[ExecuteInEditMode]
public class SceneManagerNew : MonoBehaviour
{
    //[ArrayElementTitle("SerializedName")]
    public List<SystemNode> _situations;

    //[ArrayElementTitle("SerializedName")]
    public List<SystemNode> _situations1 = new List<SystemNode>();

    private void Update()
    {
        _situations.Clear();
        List<SystemController> situationsControllers = GetComponentsInChildren<SystemController>().ToList().OrderBy(x => (int)(x.SystemNode?.Type ?? SystemType.Audio)).ToList();
        int i = 0;
        //situationsControllers.ForEach(x =>
        //{
        //    if (x.SystemNode != null)
        //    {
        //        _situations.Add(x.SystemNode);
        //        _situations.Last().Controller = x;
        //        _situations.Last().Help = x.SystemNode?.Help ?? "";
        //        _situations.Last().SerializedName = i + " - " + _situations.Last().Type.ToString() + ": " + x.gameObject.name;

        //        i++;
        //    }
        //});
    }


    public void SaveData()
    {
        NewSystemSerializer.SaveNode(_situations, "C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\data.json");
    }

    public void LoadData()
    {
        _situations1 = NewSystemSerializer.LoadNode("C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\data.json", _situations.Count);
    }
   
    
    public void SaveGraph()
    {
        NewSystemSerializer.SaveGraph(GetComponent<SceneGraph>().graph as SystemsGraph, "C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\graph.json");
    }

    public void LoadGraph()
    {
        NewSystemSerializer.LoadGraph("C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\graph.json");
    }

    public void LoadAndAssignGraph()
    {
        var graph = NewSystemSerializer.LoadGraph("C:\\NewSystemStructure\\NewSystemStructureProject\\Assets" + "\\StreamingAssets\\graph.json");
        GetComponent<SceneGraph>().graph = graph;
    }
}
