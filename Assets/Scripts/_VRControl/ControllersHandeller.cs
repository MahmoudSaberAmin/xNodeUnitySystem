using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public interface IInputHandler
{
    void OnControllerInputDown();
    void OnControllerInputUp();
}

public class ControllersHandeller : MonoBehaviour
{
    public SteamVR_Input_Sources RightHand;
    public SteamVR_Input_Sources LeftHand;
    public SteamVR_Action_Boolean PinchAction;
    public SteamVR_Action_Boolean GripAction;

    
    HighLight highLightRef;

    private GameObject _controllerTriggerObject = null;

    private List<IInputHandler> _inputHandlers = new List<IInputHandler>();

    private void Start()
    {

        PinchAction.AddOnStateDownListener(ListeningForTriggerRightHandDown, RightHand) ;
        PinchAction.AddOnStateUpListener(ListeningForTriggerRightHandUp, RightHand);
    }
    void Update()
    {
        //RightHandActions();
    }


    //this function was rotating and moving the object but rotation is deactivated

    private void ChangeObjectMaterial(GameObject obj, Material mat)
    {
        Renderer[] renderes = obj.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderes.Length; i++)
        {
            //if it is already assigned do not assign again
            if (renderes[i].material != mat)
            {
                renderes[i].material = mat;
            }
        }
    }
    //private void RightHandActions()
    //{
    //    if (PinchAction.GetStateDown(LeftHand))
    //    {
          
    //    }
    //}

    public void RighthandMovingObjectControll(RaycastHit hit, Transform parent)
    {
        
    }

    public void ObjectClicked(RaycastHit hit)
    {
        if (hit.transform?.gameObject != null)
            _controllerTriggerObject = hit.transform.gameObject;

    }

    //public GameObject GetLastClickedObject()
    //{
    //    return lastClickedGameObject;
    //}
    public void HighLight(RaycastHit hit)
    {
       
        if (hit.collider!=null&&hit.collider.GetComponent<HighLight>())
        {

            HighLight highlightTemp = hit.collider.GetComponent<HighLight>();
            //remove highlight if i'm hovering different object
            if (highLightRef != null && highlightTemp != highLightRef)
            {
                highLightRef.SetOriginalMaterialBack();
            }
            highLightRef = highlightTemp;
            //highlight object
            highlightTemp.HighLightObject();
        }
        else
        {
            //if i'm not pointing to any object and the object i was hovering still has hover material
            //set original material back
            if (highLightRef != null)
                highLightRef.SetOriginalMaterialBack();
            //remove assign from previous object
            highLightRef = null;
        }
    }
    private void ListeningForTriggerRightHandDown(SteamVR_Action_Boolean Grip, SteamVR_Input_Sources hand )
    {
        //if (_controllerTriggerObject != null)
        //    _controllerTriggerObject.GetComponent<SystemController>()?.OnControllerInputDown();
        _inputHandlers.ForEach(x=> x?.OnControllerInputDown());

    }

    private void ListeningForTriggerRightHandUp(SteamVR_Action_Boolean Grip, SteamVR_Input_Sources hand)
    {
        //if (_controllerTriggerObject != null)
        //{
        //    _controllerTriggerObject.GetComponent<SystemController>()?.OnControllerInputUp();
        //    _controllerTriggerObject = null;
        //}

        _inputHandlers.ForEach(x => x?.OnControllerInputUp());
    }

    public void Subscribe(IInputHandler inputHandler)
    {
        _inputHandlers.Add(inputHandler);
    }


    public void UnSubscribe(IInputHandler inputHandler)
    {
        _inputHandlers.Remove(inputHandler);
    }
}
