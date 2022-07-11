using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    private const float _positionYmin = 4, _positionYmax = 12;
    private const float _positionXmin = 4, _positionXmax = 12;

    private static Window _lastWindow;

    public static Window LastWindow { get => _lastWindow; }

    void Awake()
    {


        float x = Random.Range(_positionXmin, _positionXmax) * (Random.value > 0.5f ? 1 : -1);
        float y = Random.Range(_positionYmin, _positionYmax) * (Random.value > 0.5f ? 1 : -1);
        float z;
        if (_lastWindow == null)
        {
            z = Ball.Instance.Z + Random.Range(10, 30);
        }
        else
        {
            z = _lastWindow.transform.position.z + Random.Range(10, 30);
        }

        transform.position = new Vector3(x, y, z);
        transform.parent = Hexagon.Instance.transform;

        _lastWindow = this;

    }

    public static void ResetStaticValues()
    {
        _lastWindow = null;
    }


    public void BreakWindow()
    {


    }



}
