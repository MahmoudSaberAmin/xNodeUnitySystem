using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the visibility of the gameobject that is parent to the implemntation gameobject.
/// </summary>
[System.Serializable]
[NodeTint(0.3f, 0.3f, 0.4f)]
public class VisibilityNode : SystemNode
{
    /// <summary>
    /// True for actvating the gameobject, false to hide.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    [OnValueChanged("SetColor")]
    [GUIColor("GetColor")]
    public bool IsVisible;

    [HideInInspector]
    public bool _isModified = false;

    public VisibilityNode() : base()
    {
        Type = SystemType.Visibility;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<VisibilityController>();
    }

    private Color GetColor() { return this._isModified == false? Color.red : Color.white; }
    private void SetColor() { this._isModified = true; }
}