using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Playables;

[System.Serializable]
public class TimelineNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public PlayableAsset Timeline;
    public bool IsCustomStartOrEnd = false;

    [ShowIf("IsCustomStartOrEnd")]
    public float StartTime = 0f;
    [Header("0 for end of file")]
    [ShowIf("IsCustomStartOrEnd")]
    public float EndTime = 0f;
         


    public TimelineNode() : base()
    {
        Type = SystemType.Timeline;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<TimelineController>();
    }

    //public override void PutScriptsOnImplentation()
    //{
    //    base.PutScriptsOnImplentation();
    //    var src = Implementations.FirstOrDefault().AddComponent<PlayableDirector>();
    //    src.playOnAwake = false;
    //}
}