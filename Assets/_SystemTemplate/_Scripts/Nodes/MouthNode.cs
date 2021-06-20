using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
public class MouthNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public AudioClip Clip;
    public float AnimationSpeed = 1f;

    public bool IsCustomStartOrEnd = false;

    [ShowIf("IsCustomStartOrEnd")]
    public float StartTime = 0f;
    [Header("0 for end of file")]
    [ShowIf("IsCustomStartOrEnd")]
    public float EndTime = 0f;
         


    public MouthNode() : base()
    {
        Type = SystemType.Mouth;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<MouthController>();
    }

    public override void PutScriptsOnImplentation()
    {
        base.PutScriptsOnImplentation();
        var src = Implementations.FirstOrDefault().AddComponent<AudioSource>();
        
        src.playOnAwake = false;
    }
}