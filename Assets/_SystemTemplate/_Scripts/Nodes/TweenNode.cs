using UnityEngine;
using System.Linq;
using XNode;
using UnityEngine.SceneManagement;
using MyBox;
using Sirenix.OdinInspector;

public enum AnimationType
{
    None,
    Movement,
    Rotation,
    Scale,
    FadeIn,
    FadeOut,
    Shake,
}


/// <summary>
/// Performs tween animation like movement, rotation, shake, fade and scale.
/// </summary>
[System.Serializable]
[NodeTint(0.4f, 0.3f, 0.5f)]
public class TweenNode : SystemNode
{
    /// <summary>
    /// Movement, rotation, shake, fade and scale.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    [OnValueChanged("SetColor")]
    [GUIColor("GetColor")]
    public AnimationType AnimationType;
    /// <summary>
    /// True if the animation is required to loop.
    /// </summary>
    [ConditionalField("AnimationType", true, AnimationType.None)]
    public iTween.LoopType LoopType;
    /// <summary>
    /// The curve of the animation, choose EaseInOut for natural ease in ease out curve, or linear for normal curve.
    /// </summary>
    [ConditionalField("AnimationType", true, AnimationType.None)]
    public iTween.EaseType EaseType;

    /// <summary>
    /// Determine the end state for the animation, 
    ///     Use Position for Movement Animation, 
    ///     Use Rotation for Rotation Animation, 
    ///     Use Scale for Scale Animation, 
    ///     Use Scale for Shake Animation, 
    ///     Use Alpha for Fade Animation.
    /// </summary>
    [HideInInspector]
    [ConditionalField("AnimationType", true, AnimationType.None)]
    public Transform EndAnimationTransform;// End Transform for animation.

    /// <summary>
    /// true if Fade animation is required.
    /// </summary>
    [ConditionalField("AnimationType", false, AnimationType.FadeIn,AnimationType.FadeOut)]
    public bool IsImageComponent = false;
    /// <summary>
    /// The final desired alpha.
    /// </summary>
    [ConditionalField("AnimationType", false, AnimationType.FadeIn,AnimationType.FadeOut)]
    public int EndAlpha = 1;

    /// <summary>
    /// Animation duration.
    /// </summary>
    [ConditionalField("AnimationType", true, AnimationType.None)]
    public float Time = 1;


    [HideInInspector]
    public bool _isModified = false;

    public TweenNode() : base()
    {
        Type = SystemType.Tween;
        IsEndTransformAvailable = true;
    }

    /// <summary>
    /// Creates end transform.
    /// </summary>
    /// <param name="name"></param>
    public override void CreateImplementation(string name)
    {
        base.CreateImplementation(name);

        var sceneName = SceneManager.GetActiveScene().name;
        
        EndAnimationTransform = new GameObject(name + " End Transform").transform;
        EndAnimationTransform.gameObject.AddComponent<NodeTag>().TagValue = GameContstants.GetEndTransformTag(name);

        var parent = Graph.FindObjectWithTag(sceneName + "/" + name).transform;
        EndAnimationTransform.parent = parent;
    }

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<TweenController>();
    }

    /// <summary>
    /// Renames the end transform.
    /// </summary>
    /// <param name="name">node name</param>
    public override void RenameControllerAndImplementation(string name)
    {
        base.RenameControllerAndImplementation(name);

        if (EndAnimationTransform!=null)
        {
            EndAnimationTransform.name = name + " End Transform";
        }
    }


    public override Transform GetEndTransform()
    {
        return EndAnimationTransform;
    }


    private Color GetColor() { return this._isModified == false ? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }
}