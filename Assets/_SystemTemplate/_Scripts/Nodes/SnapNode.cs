using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Snap Node is used to snap a node to the position of an end transform.
/// </summary>
[System.Serializable]
public class SnapNode : SystemNode
{
    /// <summary>
    /// Transform of a gameobject to snap the implentation parent into.
    /// </summary>
    [NonSerialized]
    public Transform EndTransform;
    
    public SnapNode() : base()
    {
        Type = SystemType.Snap;
        IsEndTransformAvailable = true;
    }

    /// <summary>
    /// Creates the end transform.
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
        Controller = _triggerGameOject.AddComponent<SnapController>();
    }



    public override Transform GetEndTransform()
    {
        return EndTransform;
    }

}