using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Subclass from SystemController which plays an animation on the implementation parent.
/// </summary>
public class AnimationController : SystemController
{
    /// <summary>
    /// Logging Tag.
    /// </summary>
    private const string Tag = "AnimationController";

    /// <summary>
    /// An instance from Animation Node which is a Subclass from system Node.
    /// </summary>
    [SerializeField] private AnimationNode _assignedNode;


    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Animation node as system node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as AnimationNode;
    }

    /// <summary>
    /// The system finds an Animator instance in the parent of the implementation, if not found it adds a new animator component, it assigns the node parameters then plays the animation.
    /// </summary>
    /// <param name="other">Collided GameObject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;

        // grab the animator component
        var animator = _assignedNode.Implementations?.FirstOrDefault()?.transform.parent.GetComponent<Animator>();
        if (animator == null)
        {
            animator = _assignedNode.Implementations?.FirstOrDefault()?.transform.parent.gameObject.AddComponent<Animator>();
        }

        //// Validation
        if (_assignedNode.AnimatorController != null)
        {
            animator.runtimeAnimatorController = _assignedNode.AnimatorController;
        }

        animator.applyRootMotion = _assignedNode.IsApplyRootMotion;


        // Replace the default animation by the new animation
        if (_assignedNode.Animation != null)
        {
            AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
           
            aoc.ApplyOverrides(
                new List<KeyValuePair<AnimationClip, AnimationClip>>() 
                { 
                    new KeyValuePair<AnimationClip, AnimationClip>(aoc.animationClips[0], _assignedNode.Animation) 
                });
            animator.runtimeAnimatorController = aoc;

        }



        // End the system after the animation is done.
        if (_assignedNode?.Animation == null || _assignedNode?.Animation?.isLooping == true)
        {
            Invoke("EndSystem", 0f);
        }
        else
        {
            Invoke("EndSystem", _assignedNode.Animation.length);

        }
    }
}


