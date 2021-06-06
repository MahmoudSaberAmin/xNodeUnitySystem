using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum InputType
{
    None,
    VRInput,
    KeyboardInput,
    LeapMotionInput,
}

public static class InputFactory
{

    public static void CreatePickupInput(this IPickup pickupable,  GameObject pickupObject, InputType type)
    {
        switch (type)
        {
            case InputType.VRInput:
                CreateVRPickupInput(pickupable, pickupObject);
                break;
            case InputType.KeyboardInput:
                break;
            case InputType.LeapMotionInput:
                break;
            default:
                break;
        }
    }

    private static void CreateVRPickupInput(this IPickup pickupable, GameObject pickupObject)
    {
        if (pickupObject.GetComponent<PickupVR>()==null)
        {
            var vrPickup = pickupObject.AddComponent<PickupVR>();
            vrPickup?.StartCoroutine(vrPickup?.BindPickup(pickupable));
        }
    } 

    public static void BindGenericInput(this IInputHandler inputHandler, InputType type)
    {
        switch (type)
        {
            case InputType.VRInput:
                BindVRInput(inputHandler);
                break;
            case InputType.KeyboardInput:
                BindKeyboardInput(inputHandler);
                break;
            case InputType.LeapMotionInput:
                break;
            default:
                break;
        }
    }

    private static void BindVRInput(this IInputHandler inputHandler)
    {
        var vrInput = GameObject.FindObjectOfType<ControllersHandeller>();
        vrInput?.Subscribe(inputHandler);
    }      
    
    private static void BindKeyboardInput(this IInputHandler inputHandler)
    {

    }      
    
    
    public static void RemovePickupInput(this IPickup pickupable,  GameObject pickupObject, InputType type)
    {
        switch (type)
        {
            case InputType.VRInput:
                RemoveVRPickupInput(pickupable, pickupObject);
                break;
            case InputType.KeyboardInput:
                break;
            case InputType.LeapMotionInput:
                break;
            default:
                break;
        }
    }

    private static void RemoveVRPickupInput(this IPickup pickupable, GameObject pickupObject)
    {
       var vrSnap = pickupObject.GetComponent<PickupVR>();
        vrSnap?.RemovePickup(pickupable);
    }     
    
    public static void UnbindVRInput(this IInputHandler inputHandler, InputType type)
    {
        switch (type)
        {
            case InputType.VRInput:
                UnbindVrInput(inputHandler);
                break;
            case InputType.KeyboardInput:
                break;
            case InputType.LeapMotionInput:
                break;
            default:
                break;
        }
    }

    private static void UnbindVrInput(this IInputHandler inputHandler)
    {
        var vrInput = GameObject.FindObjectOfType<ControllersHandeller>();
        vrInput?.UnSubscribe(inputHandler);
    }   
}
