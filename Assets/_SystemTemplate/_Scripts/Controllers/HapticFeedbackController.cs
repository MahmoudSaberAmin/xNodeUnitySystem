using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Lets the controller vibrate for sometime.
/// </summary>
public class HapticFeedbackController : SystemController, ISystemValidate
{
    /// <summary>
    /// An instance from feedback node which is sub node class.
    /// </summary>
    [SerializeField] private HapticFeedbackNode _assignedNode;

    /// <summary>
    /// The Node Assigns itself to the controller.
    /// </summary>
    /// <param name="node">Feedback node as System node</param>
    public override void AssignSystemNode(SystemNode node)
    {
        base.AssignSystemNode(node);

        _assignedNode = node as HapticFeedbackNode;
    }


    /// <summary>
    /// Vibrates then ends the system.
    /// </summary>
    /// <param name="other">Collided game object</param>
    /// <returns>Coroutine</returns>
    public override IEnumerator PlaySystem(GameObject other)
    {
        yield return base.PlaySystem(other);

        if (IsValidted())
        {
            SystemNode.IsSystemPlaying = true;

            yield return StartCoroutine(Vibrate(_assignedNode.Time, 3999));
            EndSystem();
        }
    }

    /// <summary>
    /// Find the right hand controller then vibrate.
    /// </summary>
    /// <param name="length">duration of vibration</param>
    /// <param name="strength">power of vibration</param>
    /// <returns>Coroutine</returns>
    private IEnumerator Vibrate(float length, ushort strength)
    {
        var hand = FindObjectsOfType<Hand>().FirstOrDefault(x=>x.name==GameContstants.RightHandName);
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            hand?.TriggerHapticPulse(strength);
            yield return null; 
        }
    }

    /// <summary>
    /// validates time and haptic source which is the right controller's hand component.
    /// </summary>
    /// <returns>true if valid feedback source and vibration time</returns>
    public bool IsValidted()
    {
        if (_assignedNode?.Time >0)
        {
            var hapticSource = FindObjectsOfType<Hand>().FirstOrDefault(x => x.name == GameContstants.RightHandName);
            if (hapticSource == null)
            {
                Logger.Log("Error, There is no haptic source on Right Hand or right controller is off");
            }
            else
            {
                Logger.Log("Success, Haptic feedback is playing with time " + _assignedNode.Time);
                return true;
            }
        }
        else
        {
            Logger.Log("Error, There is not enough time for haptic feedback " + gameObject.name);
        }

        return false;
    }

   
}
