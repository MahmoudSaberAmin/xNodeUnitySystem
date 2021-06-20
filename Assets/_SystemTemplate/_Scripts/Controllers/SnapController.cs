using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Snap controller is a sub system controller class, it moves an object to the position and rotation of an end transform.
/// </summary>
public class SnapController : SystemController, ISystemValidate
{
    /// <summary>
    /// Instance of a snap node which is system node subclass
    /// </summary>
    [SerializeField] private SnapNode _assignedNode;

    /// <summary>
    /// Snap Node assigns itself to this controller.
    /// </summary>
    /// <param name="node"></param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as SnapNode;
    }

    /// <summary>
    /// Assigns the end transform in runtime.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    /// <summary>
    /// assign the implementation parent position and rotation of the end transform.
    /// </summary>
    /// <param name="other">Collided Game object</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;
            //Modified:Andrew
            _assignedNode.Implementations.FirstOrDefault().transform.parent.position = _assignedNode.EndTransform.position;
            _assignedNode.Implementations.FirstOrDefault().transform.parent.rotation = _assignedNode.EndTransform.rotation;
            EndSystem();
        }
    }

    /// <summary>
    /// Validates Implementations and end transform, 
    /// Implementation could be any object with the implementation tag of the node. its parent is what gets snapped.
    /// </summary>
    /// <returns>true if valid implementations and end transform</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Implementations?.FirstOrDefault() != null)
        {
            if (_assignedNode.EndTransform == null)
            {
                Logger.Log("Error, There is no SnapTo Transform for snappable to snap to");
            }
            else
            {
                Logger.Log("Success, Snappable is ready to snap at position " + _assignedNode.EndTransform.position);
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is no Snappable " + gameObject.name);
        }

        return false;
    }
}
