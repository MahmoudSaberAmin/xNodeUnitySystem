using UnityEngine;
/// <summary>
/// Used by feedback controller to give the user vibration feedback
/// </summary>
[System.Serializable]
public class HapticFeedbackNode : SystemNode
{
    /// <summary>
    /// Time for vibration.
    /// </summary>
    [Header("Special Data...")]
    [Space]
    public float Time = 1f;

    public HapticFeedbackNode() : base()
    {
        Type = SystemType.Feedback;
    }
    

    public override void PutScriptsOnController()
    {
        base.PutScriptsOnController();
        Controller = _triggerGameOject.AddComponent<HapticFeedbackController>();
    }

}