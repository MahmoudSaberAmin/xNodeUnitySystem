using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Switch case for an enum.
/// still indevelopment.
/// </summary>
public class LogicController : SystemController
{
    [SerializeField] private LogicNode _assignedNode;

    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as LogicNode;
    }

    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        _assignedNode.IsSystemPlaying = true;
        List<SystemNode> nextList = new List<SystemNode>();

        //var value = PlayerPrefs.GetString(_assignedNode.Tag.ToString() + _assignedNode.DataKey.ToString() + _assignedNode.VariableName);
        var value = PlayerPrefs.GetString(_assignedNode.VariableName);
        var output = _assignedNode.DynamicOutputs.FirstOrDefault(x => x.fieldName == value);

        if (output == null)
        {
            output = _assignedNode.GetOutputPort("Next");
        }

        for (int i = 0; i < output.ConnectionCount; i++)
        {
            var nextNode = output.GetConnection(i).node as SystemNode;
            if (nextNode != null)
            {

                nextList.Add(nextNode);
            }
        }

        foreach (var n in nextList)
        {
            if (IsNextNodeAuto(n))
            {
                ExecuteNextNode(n);
            }
        }

        EndSystem();
    }
}
