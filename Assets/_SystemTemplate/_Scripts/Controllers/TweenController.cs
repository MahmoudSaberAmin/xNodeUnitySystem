using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// A subclasss of system controller. Tween controller performs a tween animation like movement, rotation, scale, fade and shake animations.
/// </summary>
public class TweenController : SystemController
{
    /// <summary>
    /// an instance from Tween node which is a subclass from system node.
    /// </summary>
    [SerializeField] private TweenNode _assignedNode;

    /// <summary>
    /// The tween node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Tween node as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as TweenNode;
    }

    /// <summary>
    /// Assigns the end transform at the start.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndAnimationTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    /// <summary>
    /// Stops the animations on node exit.
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();
        iTween.Stop(_assignedNode?.Implementations.FirstOrDefault().transform.parent.gameObject);
        _assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject.GetComponent<FadeTween>()?.StopAllCoroutines();
    }

    /// <summary>
    /// Plays the tween animation like movement, rotation, scale, fade or shake, 
    /// 
    /// Movement depends on the position of the end transform, 
    /// Rotation depends on the eular angles of the end transform, 
    /// Scale depends on the scale of the end transform, 
    /// Fade depends on the end Alpha, 
    /// Shake depends on the scale of the end transform, 
    /// </summary>
    /// <param name="other">Collided game object</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (_assignedNode?.Implementations!=null)
        {
            switch (_assignedNode?.AnimationType)
            {
                case AnimationType.None:
                    break;
                case AnimationType.Movement:
                    iTween.MoveTo(_assignedNode?.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "position", _assignedNode?.EndAnimationTransform.position, "EaseType", _assignedNode.EaseType));
                    break;
                case AnimationType.Rotation:
                    iTween.RotateTo(_assignedNode?.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "rotation", _assignedNode?.EndAnimationTransform.eulerAngles, "EaseType", _assignedNode.EaseType));
                    break;
                case AnimationType.Scale:
                    iTween.ScaleTo(_assignedNode?.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "scale", _assignedNode?.EndAnimationTransform.localScale, "EaseType", _assignedNode.EaseType));
                    break;
                case AnimationType.FadeIn:

                    if (_assignedNode.IsImageComponent)
                    {
                        var fader = _assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject.AddComponent<FadeTween>();
                        fader.FadeTo(_assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject.GetComponent<Image>(), _assignedNode.Time, _assignedNode.EndAlpha);
                    }
                    else
                    {
                        iTween.FadeTo(_assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "alpha", _assignedNode?.EndAlpha, "EaseType", _assignedNode.EaseType));
                    }

                    break;
                case AnimationType.FadeOut:

                    if (_assignedNode.IsImageComponent)
                    {
                        var fader = _assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject.AddComponent<FadeTween>();
                        fader.FadeTo(_assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject.GetComponent<Image>(), _assignedNode.Time, _assignedNode.EndAlpha);
                    }
                    else
                    {
                        iTween.FadeTo(_assignedNode.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "alpha", _assignedNode?.EndAlpha, "EaseType", _assignedNode.EaseType));
                    }

                    break;
                case AnimationType.Shake:
                    iTween.ShakePosition(_assignedNode?.Implementations.FirstOrDefault().transform.parent.gameObject, iTween.Hash("time", _assignedNode?.Time, "LoopType", _assignedNode?.LoopType, "amount", _assignedNode?.EndAnimationTransform.localScale, "EaseType", _assignedNode.EaseType));
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(_assignedNode.Time);
            EndSystem();
        }
        else
        {
            Logger.Log("Error, There are no children for implentation");
        }
    }    
}
