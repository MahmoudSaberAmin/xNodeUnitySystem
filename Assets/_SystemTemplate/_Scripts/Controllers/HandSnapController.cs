//TODO:Andrew
//Node looping would stop the execution of the Audio node 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RootMotion.FinalIK;

/// <summary>
/// Audio controller is a subclass of system controller which plays an audio clip.
/// </summary>
public class HandSnapController : SystemController, ISystemValidate
{
    /// <summary>
    /// The audio node which is a subclass of System Node.
    /// </summary>
    [SerializeField] private HandSnapNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Audio Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as HandSnapNode;
    }

    /// <summary>
    /// Assigns the end transform in runtime.
    /// </summary>
    protected override void InitializeNodeParameters()
    {
        base.InitializeNodeParameters();
        _assignedNode.EndTransform = _assignedNode.Graph.FindObjectWithTag(GameContstants.GetEndTransformTag(_assignedNode.name))?.transform;
    }

    public bool IsValidted()
    {
        var source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<ArmIK>()?.GetIKSolver() as IKSolverArm;
        var animator = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<Animator>();

        if (animator == null)
        {
            Logger.LogError("There is no animator on character in Hand node");
        }

        if (animator.avatar == null)
        {
            Logger.LogError("There is no avatar in the animator on character in Hand node");
        }

        if (source == null)
        {
            if (_assignedNode.IsAutoFigure)
            {
                Logger.Log("Figuring arm ik");
            }
            else
            {
                Logger.LogError("Is auto figure is off and you forget to configure arm ik on the character, please put an arm ik beside the animator, Hand node error");
            }
        }

        bool isValid = (_assignedNode.IsAutoFigure == true && animator != null) || (!_assignedNode.IsAutoFigure && source != null && animator != null);

        return isValid;
    }


    /// <summary>
    /// plays the audio system with the audio node parameters (Clip).
    /// </summary>
    /// <param name="other">Collided Gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        SystemNode.IsSystemPlaying = true;
        var source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<ArmIK>()?.GetIKSolver() as IKSolverArm;
        var animator = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<Animator>();

        if (IsValidted())
        {
            if (_assignedNode.IsAutoFigure)
            {
                var hand = animator.GetBoneTransform(_assignedNode.IsLeftHand? HumanBodyBones.LeftHand: HumanBodyBones.RightHand);
                var foreArm = hand.parent;
                var upperArm = foreArm.parent;
                var shoulder = upperArm.parent;
                var lowestChest = shoulder.parent;
                var root = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent;

                source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.gameObject?.AddComponent<ArmIK>()?.GetIKSolver() as IKSolverArm;

                if (source != null)
                {
                    source.SetChain(chest: lowestChest, shoulder: shoulder, upperArm: upperArm, forearm: foreArm, hand: hand, root);
                }

                var hint = new GameObject("Hint");
                hint.transform.parent = root;
                hint.transform.position = foreArm.position;
                hint.transform.forward = root.forward;
                hint.transform.localScale = Vector3.one;

                Destroy(hint, _assignedNode.Time+0.05f);

                source.arm.bendGoal = hint.transform;
                source.arm.bendGoalWeight = 1f;
            }

            var t = 0f;
            source.isLeft = _assignedNode.IsLeftHand;

            while(t < _assignedNode.Time)
            {
                source.arm.target = _assignedNode.GetEndTransform();

                if (_assignedNode._isAnimated)
                {
                    if (t<_assignedNode.AnimationDuration)
                    {
                        source.IKPositionWeight = t/ _assignedNode.AnimationDuration;
                        source.IKRotationWeight = t / _assignedNode.AnimationDuration;
                    }
                    else
                    {
                        source.IKPositionWeight = 1f;
                        source.IKRotationWeight = 1f;
                    }
                }
                else
                {
                    source.IKPositionWeight = 1f;
                    source.IKRotationWeight = 1f;
                }


                t += Time.deltaTime;
                yield return null;
            }


            source.IKPositionWeight = 0f;
            source.IKRotationWeight = 0f;
        }
        else
        {
            Debug.LogError("Hand node error, Either check is autofigure or put armik+animator on the character");
        }

        EndSystem();
    }

}
