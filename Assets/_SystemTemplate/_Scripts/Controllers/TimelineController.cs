//TODO:Andrew
//Node looping would stop the execution of the Audio node 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Playables;

/// <summary>
/// TimelineNode controller is a subclass of system controller which plays a TimelineNode clip.
/// </summary>
public class TimelineController : SystemController, ISystemValidate
{
    /// <summary>
    /// The TimelineNode node which is a subclass of System Node.
    /// </summary>
    [SerializeField] private TimelineNode _assignedNode;

    /// <summary>
    /// The node assigns itself to its controller.
    /// </summary>
    /// <param name="node">TimelineNode Node as System Node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as TimelineNode;
    }


    /// <summary>
    /// plays the TimelineNode system with the TimelineNode node parameters (Clip).
    /// </summary>
    /// <param name="other">Collided Gameobject</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;
            var source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<PlayableDirector>();

            source.playableAsset = _assignedNode.Timeline;

            if (_assignedNode.IsCustomStartOrEnd)
            {
                source.time = _assignedNode.StartTime;
            }

            source.Play();

            if (_assignedNode.IsCustomStartOrEnd)
            {
                var length = _assignedNode.EndTime == 0 ? 
                    (float) _assignedNode.Timeline.duration - _assignedNode.StartTime :
                    _assignedNode.EndTime - _assignedNode.StartTime;
                yield return new WaitForSeconds(length);
            }
            else
            {
                yield return new WaitForSeconds((float) _assignedNode.Timeline.duration);
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

        var source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<PlayableDirector>();
        source.Stop();
        source.playableAsset = null;
    }

    /// <summary>
    /// Basic validation for the TimelineNode director and clip.
    /// </summary>
    /// <returns>true if clip & director component are valid</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Timeline != null)
        {
            var source = _assignedNode.Implementations?.FirstOrDefault()?.transform?.parent?.GetComponent<PlayableDirector>();
            if (source == null)
            {
                Logger.Log("Error, There is no Playable director on this Implementation");
            }
            else
            {
                Logger.Log("Success, Timeline is playing " + _assignedNode.Timeline);
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is no timeline assigned in node " + gameObject.name);
        }

        return false;
    }
}
