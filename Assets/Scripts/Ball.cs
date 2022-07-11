using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{


    private static Ball instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private float _speed;


    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private Collider _collider;

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
    public Rigidbody Rigidbody { get => _rigidbody; set => _rigidbody = value; }
    public MeshRenderer MeshRenderer { get => _meshRenderer; set => _meshRenderer = value; }
    public Collider Collider { get => _collider; set => _collider = value; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MeshRenderer = GetComponent<MeshRenderer>();
        Collider = GetComponent<Collider>();
    }

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


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("GameOver");
            StartCoroutine(Died());
        }

        if (other.CompareTag("Window"))
        {
            Debug.Log("Window");
            other.GetComponent<Window>().BreakWindow();
        }

    }



    private IEnumerator Died()
    {
        GameController.Instance.GameStatus = GameStatus.died;

        Collider.enabled = false;

        Rigidbody.constraints = RigidbodyConstraints.None;

        transform.DOScale(2f, 0.8f);

        yield return new WaitForEndOfFrame();
        Rigidbody.AddExplosionForce(10f, transform.position, 1f, 1f, ForceMode.VelocityChange);
       
        yield return new WaitForSeconds(0.8f);

        _meshRenderer.enabled = false;
        GameObject explosion = GameObject.Instantiate(Resources.Load("BallExplosion") as GameObject
            ,transform.position,Quaternion.identity);


        yield return new WaitForSeconds(1.1f);

        Destroy(explosion);

        Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

    }


}

