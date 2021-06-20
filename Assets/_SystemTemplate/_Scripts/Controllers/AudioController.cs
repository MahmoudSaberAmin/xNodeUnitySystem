//TODO:Andrew
//Node looping would stop the execution of the Audio node 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// Audio controller is a subclass of system controller which plays an audio clip.
/// </summary>
public class AudioController : SystemController, ISystemValidate
{
    /// <summary>
    /// The audio node which is a subclass of System Node.
    /// </summary>
    [SerializeField] private AudioNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">Audio Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as AudioNode;
    }


    /// <summary>
    /// plays the audio system with the audio node parameters (Clip).
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

            source.clip = _assignedNode.Clip;
            if (_assignedNode.IsCustomStartOrEnd)
            {
                source.time = _assignedNode.StartTime;
            }
            source.Play();
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
    /// Basic validation for the audio source and clip.
    /// </summary>
    /// <returns>true if clip & AudioSource are valid</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Clip != null)
        {
            var source = _assignedNode.Implementations?.FirstOrDefault()?.GetComponent<AudioSource>();
            if (source == null)
            {
                Logger.Log("Error, There is no audio source on this trigger");
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
