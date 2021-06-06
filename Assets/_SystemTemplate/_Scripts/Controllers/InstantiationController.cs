using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sub class of system controller which isntantaites an object in an end transform position.
/// </summary>
public class InstantiationController : SystemController, ISystemValidate
{
    /// <summary>
    /// The isntaantiation node which is a sub class of sytem node.
    /// </summary>
    [SerializeField] private InstantiationNode _assignedNode;

    /// <summary>
    /// The node assigns itself to the instantiation controller.
    /// </summary>
    /// <param name="node">Instantiation node as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as InstantiationNode;
    }
  
    /// <summary>
    /// Assigns the end transform on start.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    /// <summary>
    /// Creates the gameobject with position, rotation and scale of the end transform.
    /// </summary>
    /// <param name="other">Collided Gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;

            var go = Instantiate(_assignedNode.InstantiatedPrefab, _assignedNode.EndTransform.position, _assignedNode.EndTransform.rotation);
            go.transform.localScale = _assignedNode.EndTransform.localScale;
            EndSystem();
        }
    }


    /// <summary>
    /// Validates the prefab and the implementations.
    /// </summary>
    /// <returns>true if valid implementations and prefab</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Implementations?.FirstOrDefault()!=null)
        {
            if (_assignedNode.InstantiatedPrefab == null)
            {
                Logger.Log("Error, There is no prefab to Instantiate!");
            }
            else
            {
                Logger.Log("Success, Instation is ready");
                return true;
            }
        }
        else
        {
            Logger.Log("There is no Implementaion "  + gameObject.name);
        }

        return false;
       
    }
}
