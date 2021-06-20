using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LookNode : SystemNode
{
    [Header("Special Data...")]
    [Space]
    [FPD_Percentage(0f, 1f)]
    public float lookAmount = 1f;
    public float lookDuration = 5f;
    [System.NonSerialized]
    public Transform EndTransform;

    public LookNode() : base()
    {
        Type = SystemType.LookAt;
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
        Controller = _triggerGameOject.AddComponent<LookController>();
    }



    public override Transform GetEndTransform()
    {
        return EndTransform;
    }

}
