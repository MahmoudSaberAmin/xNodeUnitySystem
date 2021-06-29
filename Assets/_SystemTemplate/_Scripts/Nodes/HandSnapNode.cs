using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

[System.Serializable]
public class HandSnapNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    [Tooltip("Warning! Check with caution, it figures everything in runtime...")]
    public bool IsAutoFigure = false;
    public bool IsLeftHand = false;
    public float Time = 5f;

    public bool _isAnimated = false;
    [ShowIf("_isAnimated")]
    public float AnimationDuration = 1f;


    [System.NonSerialized]
    public Transform EndTransform;

    public HandSnapNode() : base()
    {
        Type = SystemType.Hand;
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
        Controller = _triggerGameOject.AddComponent<HandSnapController>();
    }


    public override Transform GetEndTransform()
    {
        return EndTransform;
    }
}