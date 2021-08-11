using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

[System.Serializable]
[NodeTint(0.4f, 0.35f, 0.1f)]
public class AudioNode : SystemNode
{
    /// <summary>
    /// It is required
    /// </summary>
    [Header("Special Data...")]
    [Space]
    [OnValueChanged("SetColor")]
    [GUIColor("GetColor")]
    public AudioClip Clip;
    public bool IsCustomStartOrEnd = false;

    [ShowIf("IsCustomStartOrEnd")]
    public float StartTime = 0f;
    [Header("0 for end of file")]
    [ShowIf("IsCustomStartOrEnd")]
    public float EndTime = 0f;



    [HideInInspector]
    public bool _isModified = false;

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


    private Color GetColor() { return this._isModified == false ? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }
}