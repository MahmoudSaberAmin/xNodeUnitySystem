using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public enum AnimationName
{
    Trigger1 = 10,
    Trigger2 = 20,
    Trigger3 = 30,
}

[System.Serializable]
[NodeTint(0, 0.3f, 0f)]
public class AnimationNode : SystemNode
{
    /// <summary>
    /// Runtime animation controller that replaces animation with the assigned one, It is required.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public RuntimeAnimatorController AnimatorController;


    [HideInInspector]
    public bool _isModified = false;

    ///// <summary>
    ///// The avatar of the character, It is optional.
    ///// </summary>
    //public Avatar AnimatorAvatar;

    ///// <summary>
    ///// Parameter of Mecanim, Must exist in Animator parameters,
    /////  We use generic Trigger1, Trigger2 Trigger3 enum to ease Data entry.
    ///// </summary>
    //public AnimationName AnimatorParamterName = AnimationName.Trigger1;

    /// <summary>
    /// The animation to be assigned to the runtime animator.
    /// </summary>
    [OnValueChanged("SetColor")]
    [GUIColor("GetColor")]
    public AnimationClip Animation;

    ///// <summary>
    ///// The speed multiplier of the animation, 
    ///// Must set multiplier in Animator, open Animator select an animation, tick the multiplier parameter, namme the multiplier to "Multiplier".
    ///// </summary>
    //public float AnimationSpeed = 1f;

    ///// <summary>
    ///// Wether to destroy the animator component on finish or to leave it.
    ///// </summary>
    //public bool IsDestroyOnFinish = true;
    public bool IsApplyRootMotion = false;

    public AnimationNode():base()
    {
        Type = SystemType.Animation;

    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();

        Controller = _triggerGameOject.AddComponent<AnimationController>();
        
    }
    public override void PutScriptsOnImplentation()
    {
        base.PutScriptsOnImplentation();
        AnimatorController = Resources.Load<RuntimeAnimatorController>(GameContstants.AnimatorControllerName);

    }



    private Color GetColor() { return this._isModified == false ? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }

}