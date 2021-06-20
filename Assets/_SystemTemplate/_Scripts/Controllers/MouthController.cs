//TODO:Andrew
//Node looping would stop the execution of the Audio node 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Mouth controller is a subclass of system controller which plays an audio clip with mouth Animation.
/// </summary>
public class MouthController : SystemController, ISystemValidate
{
    /// <summary>
    /// The mouth node which is a subclass of System Node.
    /// </summary>
    [SerializeField] private MouthNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Mouth Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);
        _assignedNode = node as MouthNode;
    }


    /// <summary>
    /// plays the audio system with the audio node parameters (Clip) with mouth animation.
    /// </summary>
    /// <param name="other">Collided Gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;
            var source = _assignedNode.Implementations?.FirstOrDefault()?.GetComponent<AudioSource>();
            var animator = _assignedNode.Implementations?.FirstOrDefault()?.transform.parent.GetComponent<Animator>();

            if (animator==null)
            {
                animator = _assignedNode.Implementations?.FirstOrDefault()?.transform.parent.gameObject.AddComponent<Animator>();
            }

            animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(GameContstants.AnimatorControllerName);

            source.clip = _assignedNode.Clip;
            if (_assignedNode.IsCustomStartOrEnd)
            {
                source.time = _assignedNode.StartTime;
            }

            source.Play();
            animator?.SetBool(GameContstants.MecanimMouthTalkingParameter, true);
            animator?.SetFloat(GameContstants.MecanimMouthSpeedParameter, _assignedNode.AnimationSpeed);

            if (_assignedNode.IsCustomStartOrEnd)
            {
                var length = _assignedNode.EndTime == 0 ? 
                    _assignedNode.Clip.length - _assignedNode.StartTime :
                    _assignedNode.EndTime - _assignedNode.StartTime;
                yield return new WaitForSeconds(length);
            }
            else
            {
                yield return new WaitForSeconds(_assignedNode.Clip.length);
            }
    
            animator?.SetBool(GameContstants.MecanimMouthTalkingParameter, false);
            EndSystem();
        }
    }

    /// <summary>
    /// Removes clip on exit.
    /// </summary>
    protected override void OnExit()
    {
        base.OnExit();

        var source = _assignedNode.Implementations?.FirstOrDefault()?.GetComponent<AudioSource>();
        source.clip = null;
    }

    /// <summary>
    /// Basic validation for the audio source, animator and clip.
    /// </summary>
    /// <returns>true if clip & AudioSource are valid</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Clip != null)
        {
            var source = _assignedNode.Implementations?.FirstOrDefault()?.GetComponent<AudioSource>();
            var animator = _assignedNode.Implementations?.FirstOrDefault()?.transform.parent.GetComponent<Animator>();

            if (source == null)
            {
                Logger.Log("Error, There is no audio source found, Mouth Node");
            }
            else if (animator == null)
            {
                Logger.Log("Error, There is no Animator source found, Mouth Node");
            }
            else
            {
                Logger.Log("Success, Audio Source is playing " + _assignedNode.Clip);
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is no clip " + gameObject.name);
        }

        return false;
    }

   
}
