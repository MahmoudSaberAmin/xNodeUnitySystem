using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum InputsType
{
    None,
    VRInput,
    KeyboardInput,
    LeapMotionInput,
}

public static class InputFactory
{

    public static void CreatePickupInput(this IPickup pickupable,  GameObject pickupObject, string typeStr)
    {
        var type = Enum.Parse(typeof(InputsType), typeStr);
        switch (type)
        {
            case InputsType.VRInput:
                CreateVRPickupInput(pickupable, pickupObject);
                break;
            case InputsType.KeyboardInput:
                break;
            case InputsType.LeapMotionInput:
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

    public static void BindGenericInput(this IInputHandler inputHandler, string typeStr)
    {
        var type = Enum.Parse(typeof(InputsType), typeStr);
        switch (type)
        {
            case InputsType.VRInput:
                BindVRInput(inputHandler);
                break;
            case InputsType.KeyboardInput:
                BindKeyboardInput(inputHandler);
                break;
            case InputsType.LeapMotionInput:
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
    
    
    public static void RemovePickupInput(this IPickup pickupable,  GameObject pickupObject, string typeStr)
    {
        var type = Enum.Parse(typeof(InputsType), typeStr);
        switch (type)
        {
            case InputsType.VRInput:
                RemoveVRPickupInput(pickupable, pickupObject);
                break;
            case InputsType.KeyboardInput:
                break;
            case InputsType.LeapMotionInput:
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
    
    public static void UnbindVRInput(this IInputHandler inputHandler, string typeStr)
    {
        var type = Enum.Parse(typeof(InputsType), typeStr);
        switch (type)
        {
            case InputsType.VRInput:
                UnbindVrInput(inputHandler);
                break;
            case InputsType.KeyboardInput:
                break;
            case InputsType.LeapMotionInput:
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
