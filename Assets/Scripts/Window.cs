using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour
{

    private const float _positionYmin = 3, _positionYmax = 6;
    private const float _positionXmin = 3, _positionXmax = 6;

    private static Window _lastWindow;

    public static Window LastWindow { get => _lastWindow; }
    public Material CurrentMaterial { get => _currentMaterial; set => _currentMaterial = value; }

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

    private Material _currentMaterial;

    private void Start()
    {
        string materialName = Ball.Instance.GetRandomMaterial();
        _currentMaterial = Ball.Instance.MaterialsDictionary[materialName];

        foreach (Transform child in transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            meshRenderer.material = _currentMaterial;
            Color color = meshRenderer.material.color;
            color.a = 0.3f;
            meshRenderer.material.color = color;
        }
    }



    private int delay;
    private void Update()
    {
        delay++;
        if (delay >= 10)
        {
            if (transform.position.z < Ball.Instance.Z - 4)
            {
                this.gameObject.SetActive(false);
            }
            delay = 0;
        }

    }

    public static void ResetStaticValues()
    {
        _lastWindow = null;
    }



    private bool broken;
    public void BreakWindow()
    {
        if (broken)
            return;
        StartCoroutine(BreakRoutine());
    }

    private IEnumerator BreakRoutine()
    {
        broken = true;

        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(20f, transform.position, 0.6f, 0.6f, ForceMode.VelocityChange);
            //rb.transform.DOScale(0.2f, 0.5f);
        }

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
