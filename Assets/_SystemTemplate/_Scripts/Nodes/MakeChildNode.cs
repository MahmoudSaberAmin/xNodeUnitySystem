using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakeChildNode : SystemNode
{

    [NonSerialized]
    public Transform EndTransform;

    public bool IsFollowPos;
    public bool IsFollowRot;


    public MakeChildNode() : base ()
    {
        Type = SystemType.MakeChild;
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
        Controller = _triggerGameOject.AddComponent<MakeChildController>();
    }



    public override Transform GetEndTransform()
    {
        return EndTransform;
    }



}
