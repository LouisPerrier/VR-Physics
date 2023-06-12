using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KMConstantScript : MonoBehaviour
{

    [SerializeField] private Slider _sliderA;
    [SerializeField] private Slider _sliderK;
    [SerializeField] private Slider _sliderM;
    [SerializeField] private GameObject _cube;
    [SerializeField] private GameObject _ghost;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    private Vector3 _initialPos;
    private bool _started = false;
    private float _time;
    private float _k;
    private float _a;
    private float _m;
    private float A = KeyboardReader.A;
    private float K = KeyboardReader.k;
    private float M = KeyboardReader.m;

    // Start is called before the first frame update
    void Start()
    {
        _initialPos = _cube.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_started)
        {
            float newPos = _initialPos.x + _a * Mathf.Sin(Mathf.Sqrt(_k / _m) * _time);
            float truePos = _initialPos.x + A * Mathf.Sin(Mathf.Sqrt(K / M) * _time);
            _cube.GetComponent<Rigidbody>().MovePosition(new Vector3(newPos, _initialPos.y, _initialPos.z));
            if (newPos == truePos)
            {
                _ghost.GetComponent<MeshRenderer>().material = _greenMaterial;
            }
            else
            {
                _ghost.GetComponent<MeshRenderer>().material = _redMaterial;
            }
            _ghost.GetComponent<Rigidbody>().MovePosition(new Vector3(truePos, _initialPos.y, _initialPos.z));
            _time += Time.deltaTime;
        }
    }

    public void ApplyForce()
    {
        _started = true;
        _time = 0;
        _a = _sliderA.value;
        _k = _sliderK.value;
        _m = _sliderM.value;
        _ghost.transform.parent = null;
        _ghost.SetActive(true);
    }

    public void ResetLevel()
    {
        _started = false;
        _cube.transform.position = _initialPos;
        _sliderA.value = 0;
        _sliderK.value = 0;
        _sliderM.value = 0;
        _ghost.transform.parent = transform;
        _ghost.SetActive(false);
    }
}
