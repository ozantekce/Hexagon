using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private float _time, _speed = 2;

    void Update()
    {
        float cameraZ;
        if (_time < 1)
        {
            _time += Time.deltaTime * _speed;
            cameraZ = Mathf.Lerp(transform.position.z, -15f, _time);
        }
        else
        {
            cameraZ = Ball.Instance.Z - 15f;
        }

        cameraZ = Ball.Instance.Z - 15f;

        transform.position = new Vector3(0, 0, cameraZ);

    }


}
