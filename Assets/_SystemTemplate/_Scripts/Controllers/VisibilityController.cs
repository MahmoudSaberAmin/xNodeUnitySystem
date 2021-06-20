using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This system is a sub class from system controller. it controls the visibity of a gameobject.
/// </summary>
public class VisibilityController : SystemController
{
    /// <summary>
    /// An instance of Visibility node whic is a system node subclass.
    /// </summary>
    [SerializeField] private VisibilityNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Visibility node as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);
        _assignedNode = node as VisibilityNode;
    }

    /// <summary>
    /// Sets the visibility of the gameobject, 
    /// If the impelemntation parent is the node, log an error, 
    /// If the implementation parent is an object set active to false.
    /// </summary>
    /// <param name="other">Collided Game object</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);
        _assignedNode.IsSystemPlaying = true;

        foreach (var go in _assignedNode?.Implementations)
        {
            var parentObject = go.transform.parent;
            if (parentObject == transform.parent)
            {
                Logger.LogError("Implementation of Visibility Node exists in the Graph Hirearchy not as a child of a gameobject " + go.name);
            }
            else
            {
                parentObject.gameObject.SetActive(_assignedNode.IsVisible);
            }
        }
        yield return null;
        EndSystem();
    }   
}
