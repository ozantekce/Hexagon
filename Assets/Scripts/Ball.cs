using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class Ball : MonoBehaviour
{

    private static Ball instance;

    private void Awake()
    {
        instance = this;
        _materialsDictionary = new Dictionary<string, Material>();
        for (int i = 0; i < _materials.Length; i++)
        {
            _materialsDictionary.Add(_materials[i].name, _materials[i].material);
        }

    }

    [SerializeField]
    private float _speed;



    private Material _currentMaterial;
    private Dictionary<string, Material> _materialsDictionary;
    [SerializeField]
    private MaterialInfo[] _materials;


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
    public Material CurrentMaterial { get => _currentMaterial; set => _currentMaterial = value; }
    public Dictionary<string, Material> MaterialsDictionary { get => _materialsDictionary; set => _materialsDictionary = value; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MeshRenderer = GetComponent<MeshRenderer>();
        Collider = GetComponent<Collider>();

        ChangeMaterial(GetRandomMaterial());

    }

    void Update()
    {

        if(GameController.Instance.CurrentStatus == GameStatus.waitToPlay)
        {
            
            if (Touch.Pressing)
            {
                GameController.Instance.ChangeGameStatus(GameStatus.playing);
            }
        }

        if(GameController.Instance.CurrentStatus == GameStatus.levelUp)
            GoForward();

        if (GameController.Instance.CurrentStatus != GameStatus.playing)
        {
            Ball.Instance.Rigidbody.velocity = Vector3.zero;
            return;
        }


        if (Touch.Pressing)
        {
            float speed = Hexagon.Instance.MaxRotationSpeed 
                * (0.5f + (Touch.TouchDistanceToCenter()) / (Screen.width / 2));
            Hexagon.Instance.Rotate(Touch.TouchDirection(), speed);
        }

        GoForward();



    }



    public void ChangeMaterial(string name)
    {
        CurrentMaterial = _materialsDictionary[name];
        MeshRenderer.material = CurrentMaterial;
    }

    public string GetRandomMaterial()
    {
        int index = Random.Range(0,_materialsDictionary.Count);
        return _materialsDictionary.Keys.ElementAt(index);
    }


    private void GoForward()
    {

        float z = Speed * Time.deltaTime;
        transform.position += new Vector3(0,0,z); 

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
            Window window = other.GetComponent<Window>();

            if (CurrentMaterial.Equals(window.CurrentMaterial))
            {
                other.GetComponent<Window>().BreakWindow();
            }
            else
            {
                Debug.Log("GameOver");
                StartCoroutine(Died());
            }
            


        }


        if (other.CompareTag("FinishLine"))
        {
            StartCoroutine(LevelUp());
        }

    }



    private IEnumerator Died()
    {
        GameController.Instance.ChangeGameStatus(GameStatus.died);

        Collider.enabled = false;

        Rigidbody.constraints = RigidbodyConstraints.None;

        transform.DOScale(2f, 0.8f);

        yield return new WaitForEndOfFrame();
        Rigidbody.AddExplosionForce(10f, transform.position, 1f, 1f, ForceMode.VelocityChange);
       
        yield return new WaitForSeconds(0.8f);

        _meshRenderer.enabled = false;
        GameObject explosion = GameObject.Instantiate(Resources.Load("BallExplosion") as GameObject
            ,transform.position,Quaternion.identity);


        foreach (Transform child in explosion.transform)
        {
            child.GetComponent<ParticleSystem>().startColor = CurrentMaterial.color;
        }

        yield return new WaitForSeconds(1.1f);

        Destroy(explosion);

        Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
        GameController.Instance.ChangeGameStatus(GameStatus.menu);
    }


    private IEnumerator LevelUp()
    {
        GameController.Instance.Level++;
        Collider.enabled = false;
        Rigidbody.isKinematic = true;
        CameraFollow.Following = false;
        GameController.Instance.ChangeGameStatus(GameStatus.levelUp);
        yield return new WaitForSeconds(2f);
        GameController.Instance.ChangeGameStatus(GameStatus.waitToPlay);
        CameraFollow.Following = true;
        Rigidbody.isKinematic = false;


    }



    [Serializable]
    struct MaterialInfo
    {
        public string name;
        public Material material;
    }

}

