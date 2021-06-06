using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the visibility of the gameobject that is parent to the implemntation gameobject.
/// </summary>
[System.Serializable]
public class VisibilityNode : SystemNode
{
    /// <summary>
    /// True for actvating the gameobject, false to hide.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public bool IsVisible;

    public VisibilityNode() : base()
    {
        Type = SystemType.Visibility;
    }


    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<VisibilityController>();
    }
}