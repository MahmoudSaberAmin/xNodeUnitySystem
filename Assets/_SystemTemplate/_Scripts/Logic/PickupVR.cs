using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class PickupVR :MonoBehaviour
{
    public IEnumerator BindPickup(IPickup pickupable)
    {
        yield return new WaitForSeconds(0.1f);

        Collider collider = gameObject.GetComponent<Collider>();
        if (collider==null)
        {
            collider = gameObject.AddComponent<BoxCollider>();
        }

        Throwable throwable = gameObject.GetComponent<Throwable>();
        if (throwable == null)
        {
            throwable = gameObject.AddComponent<Throwable>();
        }

        throwable.onPickUp = new UnityEvent();// Work Around 
        throwable.onDetachFromHand = new UnityEvent();// Work Around 

        throwable.onPickUp.AddListener(pickupable.Pickup);
        throwable.onDetachFromHand.AddListener(pickupable.Detach);

        GetComponent<Rigidbody>().mass = 0.1f;
        GetComponent<Rigidbody>().isKinematic = false;
    }    
    
    public void RemovePickup(IPickup snappable)
    {
        GetComponent<Throwable>().onPickUp.RemoveListener(snappable.Pickup);
        GetComponent<Throwable>().onDetachFromHand.RemoveListener(snappable.Detach);

        GetComponent<Rigidbody>().isKinematic = true;

        DestroyImmediate(GetComponent<Throwable>());
        DestroyImmediate(GetComponent<Interactable>());
        DestroyImmediate(this);
    }
}
