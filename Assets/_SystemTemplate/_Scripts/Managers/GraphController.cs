using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GraphController : MonoBehaviour
{
    [FormerlySerializedAs("_graph")]
    public SystemsGraph Graph;
    // Start is called before the first frame update
    void Awake()
    {
        FigureUpStartup();
    }




    public void AssignGraphToController(SystemsGraph graph)
    {
        Graph = graph;
    }


    private void FigureUpStartup()
    {
        Graph.current.Clear();
        var nodesHavingStart = Graph.nodes.Where(x =>
        {
            var start = (x.GetInputPort("Previous")?.Connection?.node as StartNode);
            return start != null && start.IsEnabled == true;

        }).Select(x=> x as SystemNode).ToList();
        Graph.current.AddRange(nodesHavingStart);
    }
}
