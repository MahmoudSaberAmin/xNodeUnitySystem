using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This node is used by the pickup controller which takes the parent of the implementation, and attach a throwable to it, 
/// The player holds the object and can release it, 
/// If it is released near the end transform it auto snaps to it.
/// </summary>
[System.Serializable]
[NodeTint(0.4f, 0.45f, 0.6f)]
public class PickupNode : SystemNode
{
    /// <summary>
    /// The minimum distannace to snap, 
    /// The controller draws it in the scene for debugging purpose.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public float MinDistance = 0.3f;
    /// <summary>
    /// Transform to snap to
    /// </summary>
    [NonSerialized] public Transform EndTransform;



    [HideInInspector]
    public bool _isModified = false;


    /// <summary>
    /// Sets some default parameters like Input type and trigger tag.
    /// </summary>
    public PickupNode() : base()
    {
        Type = SystemType.Pickup;
        IsEndTransformAvailable = true;
        IsColorInput = true;
        InputType = InputsType.VRInput.ToString();
        //TriggerTag = "RightHand";
    }

    /// <summary>
    /// Creates an Extra End Transform.
    /// </summary>
    /// <param name="name">node name</param>
    public override void CreateImplementation(string name)
    {
        base.CreateImplementation(name);

        var sceneName = SceneManager.GetActiveScene().name;

        EndTransform = new GameObject(name + " End Transform").transform;
        EndTransform.gameObject.AddComponent<NodeTag>().TagValue = GameContstants.GetEndTransformTag(name);

        var parent = Graph.FindObjectWithTag(sceneName + "/" + name).transform;
        EndTransform.parent = parent;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<PickupController>();
    }

    public override Transform GetEndTransform()
    {
        return EndTransform;
    }


    private Color GetColor() { return this._isModified == false ? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }
}