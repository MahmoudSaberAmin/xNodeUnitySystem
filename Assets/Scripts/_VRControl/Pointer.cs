using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

public class Pointer : MonoBehaviour
{
    public float LazerDefaultLength;
    public GameObject LazerDot;
    public VR_InputModule InputModuleRef;
    public GameObject RightHand;
    private LineRenderer LazerLineRenderer = null;

    [SerializeField]
    private LayerMask _layerMaskTrigger;


    [SerializeField] private ControllersHandeller controller;
    void Start()
    {
        LazerLineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {

        PointerEventData data = InputModuleRef.GetData();

        float targetLength = data.pointerCurrentRaycast.distance == 0 ? LazerDefaultLength : data.pointerCurrentRaycast.distance;
        
        RaycastHit hit = CreateRaycast(targetLength);
        ///RaycastHit hit = CreateRaycastIgnoringNoneSystemObjects(targetLength);
        

        controller.ObjectClicked(hit);
        
        //controller.HighLight(hit); //To be used then

        DrawLaser(targetLength, hit);
    }
    
    private RaycastHit CreateRaycastIgnoringNoneSystemObjects(float length)
    {
        RaycastHit targetHit = new RaycastHit();
        Ray ray = new Ray(transform.position, transform.forward);

        //check if there is an object with SystemTrigger script if founded assign to targeted raycast
        RaycastHit[] hits = Physics.RaycastAll(ray, length, _layerMaskTrigger);
        foreach (var hit in hits)
        {
            if (hit.collider.GetComponent<SystemController>() )
            {
                targetHit = hit;
                break;
            }
        }

        return targetHit;
    }
    

    
    private void DrawLaser(float targetLength, RaycastHit hit)
    {
        Vector3 endPosiion = transform.position + (transform.forward * targetLength);

        if (hit.collider != null)
            endPosiion = hit.point;

        LazerDot.transform.position = endPosiion;
        LazerDot.transform.rotation = RightHand.transform.rotation;
        LazerLineRenderer.SetPosition(0, transform.position);
        LazerLineRenderer.SetPosition(1, endPosiion);
    }

    /* For Normal Behavior of Created Raycast*/
    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit targetHit;

        Physics.Raycast(transform.position, transform.forward, out targetHit, length);
        return targetHit;
    }
    
}
