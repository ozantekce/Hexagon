using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{


    private static Ball instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private float _speed;
    

    public float Speed { get => _speed; set => _speed = value; }
    public float Z { 
        get {
            return transform.position.z;
        } 
        set { 
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }
    }

    public static Ball Instance { get => instance; set => instance = value; }

    void Update()
    {

        if(GameController.Instance.GameStatus == GameStatus.waitForMove)
        {
            if (Touch.Pressing)
            {
                GameController.Instance.GameStatus = GameStatus.moving;
            }
        }

        if (GameController.Instance.GameStatus != GameStatus.moving)
        {
            return;
        }


        if (Touch.Pressing)
        {
            float speed = Hexagon.Instance.MaxRotationSpeed * (TouchDistanceToCenter()) / (Screen.width / 2);
            Hexagon.Instance.Rotate(TouchDirection(), speed);
        }

        GoForward();



    }


    private void GoForward()
    {

        float z = Speed * Time.deltaTime;
        transform.position += new Vector3(0,0,z); 

    }



    private float TouchDistanceToCenter()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;

        //Debug.Log(mousePos.x + " " + Screen.width/2);
        float dis = Mathf.Abs(mousePos.x);
        return Mathf.Clamp(dis, 0, Screen.width / 2);
    }

    private RotateDirection TouchDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;

        float gap = 10f;
        if (Mathf.Abs(mousePos.x) < gap)
        {
            return RotateDirection.none;
        }
        else if(mousePos.x > 0)
        {
            return RotateDirection.right;
        }
        else
        {
            return RotateDirection.left;
        }

    }


}

