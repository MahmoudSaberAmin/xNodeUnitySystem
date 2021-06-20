using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using TMPro;
public class VR_InputModule : BaseInputModule
{
    public Camera m_camera;
    public SteamVR_Input_Sources m_TargetSource;
    public SteamVR_Action_Boolean m_ClickAction;

    private GameObject m_CurrentObject = null;
    private PointerEventData m_Data;
    private GameObject newPointerPress;

    protected override void Awake()
    {
        base.Awake();
        m_Data = new PointerEventData(eventSystem);
    }

    public PointerEventData GetData()
    {
        return m_Data;
    }
    public override void Process()
    {
        m_Data.Reset();
        m_Data.position = new Vector2(m_camera.pixelWidth / 2, m_camera.pixelHeight / 2);
        eventSystem.RaycastAll(m_Data, m_RaycastResultCache);
        m_Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_CurrentObject = m_Data.pointerCurrentRaycast.gameObject;
        //cleare Cach
        m_RaycastResultCache.Clear();
        //Hover
        HandlePointerExitAndEnter(m_Data, m_CurrentObject);
        //Press
        if (m_ClickAction.GetStateDown(m_TargetSource))
        {
            ProcessPress(m_Data);
        }

        if (m_ClickAction.GetStateUp(m_TargetSource))
        {
            ProcessRelease(m_Data);
        }
        //if (m_ClickAction.GetState(m_TargetSource))
        //{
        //    newPointerPress = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(m_CurrentObject);
        //    if (newPointerPress!=null)
        //    {
        //        if (newPointerPress.GetComponent<Slider>())
        //        {
        //            AxisEventData axis = new AxisEventData(eventSystem);
        //            axis.moveDir = MoveDirection.Right;
        //            newPointerPress.GetComponent<Slider>().OnPointerUp(m_Data);
        //            newPointerPress.GetComponent<Slider>().OnDeselect(m_Data);
        //            newPointerPress.GetComponent<Slider>().OnInitializePotentialDrag(m_Data);
        //            newPointerPress.GetComponent<Slider>().OnDrag(m_Data);
        //            newPointerPress.GetComponent<Slider>().OnMove(axis);
        //            newPointerPress.GetComponent<Slider>().value += m_Data.scrollDelta.x;
        //        }
        //    }

        //}
    }
    private void ProcessPress(PointerEventData data)
    {
        newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(m_CurrentObject);
        GameObject newPointerhover = ExecuteEvents.GetEventHandler<IScrollHandler>(m_CurrentObject);
        if (newPointerhover!=null)
        {
            if (newPointerhover.GetComponent<EventTrigger>())
            {
                newPointerhover.GetComponent<EventTrigger>().OnDrag(data);
            }
        }
        if (newPointerPress != null)
        {
            if (newPointerPress.GetComponent<Button>())
            {
                newPointerPress.GetComponent<Button>().OnPointerDown(data);
            }
            else if (newPointerPress.GetComponent<Toggle>())
            {
                newPointerPress.GetComponent<Toggle>().OnPointerDown(data);
            }
            else if (newPointerPress.GetComponent<TMP_Dropdown>())
            {
                newPointerPress.GetComponent<TMP_Dropdown>().OnPointerDown(data);
            }
            else if (newPointerPress.GetComponent<Slider>())
            {
                newPointerPress.GetComponent<Slider>().OnPointerDown(data);
            }
        }

    }

    private void ProcessRelease(PointerEventData data)
    {
        if (newPointerPress != null)
        {
            if (newPointerPress.GetComponent<Button>())
            {
                newPointerPress.GetComponent<Button>().OnPointerUp(data);
                newPointerPress.GetComponent<Button>().OnPointerClick(data);
                newPointerPress.GetComponent<Button>().OnDeselect(data);
            }
            else if (newPointerPress.GetComponent<Toggle>())
            {
                newPointerPress.GetComponent<Toggle>().OnPointerUp(data);
                newPointerPress.GetComponent<Toggle>().OnPointerClick(data);
                newPointerPress.GetComponent<Toggle>().OnDeselect(data);
            }
            else if (newPointerPress.GetComponent<TMP_Dropdown>())
            {
                newPointerPress.GetComponent<TMP_Dropdown>().OnPointerUp(data);
                newPointerPress.GetComponent<TMP_Dropdown>().OnPointerClick(data);
                newPointerPress.GetComponent<TMP_Dropdown>().OnDeselect(data);
            }
            else if(newPointerPress.GetComponent<TMP_InputField>())
            {
                newPointerPress.GetComponent<TMP_InputField>().OnPointerDown(data);
            }


        }

    }
}
