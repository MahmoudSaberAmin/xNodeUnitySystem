using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Instantiates a prefab to the end transform position, rotation and scale.
/// </summary>
public class InstantiationNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public GameObject InstantiatedPrefab;
    /// <summary>
    /// Position, rotation and scale Transform.
    /// </summary>
    [NonSerialized]
    public Transform EndTransform;
    public InstantiationNode () : base()
    {
        Type = SystemType.Instantiation;
        IsEndTransformAvailable = true;
    }

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
        Controller = _triggerGameOject.AddComponent<InstantiationController>();
    }


    public override Transform GetEndTransform()
    {
        return EndTransform;
    }

}
