using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillationFormulaScript : FormulaControlled
{
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private GameObject _cube;
    [SerializeField] private GameObject _ghost;

    private Vector3 _initialPos;
    private bool _started = false;
    private float _time;

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
            float newPos = _initialPos.x + KeyboardReader.getFloatFromFormula(Formula, _time);
            float truePos = _initialPos.x + KeyboardReader.A * Mathf.Sin(Mathf.Sqrt(KeyboardReader.k / KeyboardReader.m) * _time);
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
        if (Formula != null)
        {
            _started = true;
            _time = 0;
            _ghost.transform.parent = null;
            _ghost.SetActive(true);
        }
    }

    public void ResetLevel()
    {
        _started = false;
        _cube.transform.position = _initialPos;
        _ghost.transform.parent = transform;
        _ghost.SetActive(false);
    }

    public override void UpdateFormula(Queue<string> formula)
    {
        Formula = formula;
    }
}
