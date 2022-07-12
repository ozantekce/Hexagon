using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{


    private const float _positionYmin = 4, _positionYmax =12;
    private const float _positionXmin = 4, _positionXmax = 12;

    private static Obstacle _lastObstacle;

    public static Obstacle LastObstacle { get => _lastObstacle;}

    void Awake()
    {


        float x = Random.Range(_positionXmin,_positionXmax) * (Random.value >0.5f  ? 1 : -1);
        float y = Random.Range(_positionYmin,_positionYmax) * (Random.value > 0.5f ? 1 : -1);
        float z;
        if(_lastObstacle == null)
        {
            z = Ball.Instance.Z + Random.Range(10,30);
        }
        else
        {
            z = _lastObstacle.transform.position.z + Random.Range(10, 30);
        }

        transform.position = new Vector3(x, y, z);
        transform.parent = Hexagon.Instance.transform;
        
        _lastObstacle = this;

    }


    private int delay;
    private void Update()
    {
        delay++;
        if(delay >= 10)
        {
            if(transform.position.z < Ball.Instance.Z-4)
            {
                this.gameObject.SetActive(false);
            }
            delay = 0;
        }

    }

    public static void ResetStaticValues()
    {
        _lastObstacle = null;

    }



}
