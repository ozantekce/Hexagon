using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Touch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private static bool _pressing;

    private static bool _active;

    public static bool Active { get => _active; set => _active = value; }
    public static bool Pressing { get => _pressing; set => _pressing = value; }

    public void OnPointerDown(PointerEventData eventData)
    {

        _pressing = true;


    }

    public void OnPointerUp(PointerEventData eventData)
    {

        _pressing = false;


    }




}
