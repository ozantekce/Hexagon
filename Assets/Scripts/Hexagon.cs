using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{


    private static Hexagon instance;


    [SerializeField]
    private float _maxRotationSpeed;

    public float MaxRotationSpeed { get => _maxRotationSpeed; set => _maxRotationSpeed = value; }
    public static Hexagon Instance { get => instance; set => instance = value; }

    void Awake()
    {
        instance = this;
    }


    private RotateDirection lastDirection;
    private void Update()
    {

        /*
        if(GameController.Instance.CurrentStatus == GameStatus.playing)
            Rotate(lastDirection, 5f);
        */

    }
    public void Rotate(RotateDirection direction,float speed)
    {

        speed = Mathf.Clamp(speed, 0, _maxRotationSpeed);

        if(direction == RotateDirection.left)
        {
            transform.Rotate(0f, 0f, speed * Time.deltaTime);
            lastDirection = direction;
        }
        else if(direction == RotateDirection.right)
        {
            transform.Rotate(0f, 0f, -speed * Time.deltaTime);
            lastDirection = direction;
        }

        

    }

}

public enum RotateDirection
{
    left,right,none
}