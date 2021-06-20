using FIMSpace.FLook;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILookAt 
{
    Transform LookAtTransform { get; set; }
    void LookAtWithGameobject(GameObject looker, float amount);
}


public class LookAtSomething : ILookAt
{
    public Transform LookAtTransform { get; set; }

    public void LookAtWithGameobject(GameObject looker, float amount)
    {
        if (looker==null)
        {
            return;
        }

        var lookAtScript = looker.GetComponentInChildren<FLookAnimator>();
        lookAtScript.LookAnimatorAmount = amount;

        if (lookAtScript!=null)
        {
            if (LookAtTransform == null)
            {
                lookAtScript.ObjectToFollow = null;
                 
            }
            else
            {
                lookAtScript.ObjectToFollow = LookAtTransform;
            }
        }
    }
}