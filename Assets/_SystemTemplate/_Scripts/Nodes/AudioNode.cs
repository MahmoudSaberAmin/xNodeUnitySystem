using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
public class AudioNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public AudioClip Clip;
    public bool IsCustomStartOrEnd = false;

    [ShowIf("IsCustomStartOrEnd")]
    public float StartTime = 0f;
    [Header("0 for end of file")]
    [ShowIf("IsCustomStartOrEnd")]
    public float EndTime = 0f;
         


    public AudioNode() : base()
    {
        Type = SystemType.Audio;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<AudioController>();
    }

    public override void PutScriptsOnImplentation()
    {
        base.PutScriptsOnImplentation();
        var src = Implementations.FirstOrDefault().AddComponent<AudioSource>();
        src.playOnAwake = false;
    }
}