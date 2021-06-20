using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Instantiates a model and changes its material color.
/// </summary>
public class ModelController : SystemController, ISystemValidate
{
    /// <summary>
    /// An instance from Model node which is a system node subclass.
    /// </summary>
    [SerializeField] private ModelNode _assignedNode;

    /// <summary>
    /// The model node assigns itself to its controller.
    /// </summary>
    /// <param name="node">ModelNode as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as ModelNode;
    }

    /// <summary>
    /// True if model prefab is not null.
    /// </summary>
    /// <returns>true if prefab is valid</returns>
    public bool IsValidted()
    {
        return _assignedNode?.ModelPrefab != null;
    }

    /// <summary>
    /// Creates an instance from the model prefab with the implementation position, rotation and scale and sets the first material material color in order to skin the gameobject.
    /// This is a legacy system feature port.
    /// </summary>
    /// <param name="other">Collided game object</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;

        var go = Instantiate(_assignedNode.ModelPrefab);

        go.transform.position = _assignedNode.Implementations?.FirstOrDefault()?.transform.position??Vector3.zero;
        go.transform.rotation = _assignedNode.Implementations?.FirstOrDefault()?.transform.rotation??Quaternion.identity;
        go.transform.localScale = _assignedNode.Implementations?.FirstOrDefault()?.transform.localScale ?? Vector3.one;

        MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();
        go.GetComponentInChildren<Renderer>()?.GetPropertyBlock(_propBlock);
        _propBlock?.SetColor("_Color", _assignedNode.Color);
        go.GetComponentInChildren<Renderer>()?.SetPropertyBlock(_propBlock);

        go.SetActive(false);

        go.name = "Model3d";

        go.transform.SetParent(_assignedNode.Implementations.FirstOrDefault()?.transform);

        Logger.Log("Sucessfully Instantiated " + _assignedNode.Implementations.FirstOrDefault()?.name);
        EndSystem();
    }
}
