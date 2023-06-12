using System.Collections.Generic;
using UnityEngine;

public class GrabInteraction : FormulaControlled
{
    public Material greenMaterial;
    public Material redMaterial;

    [SerializeField] private SphereRespawnManager _sphereRespawnManager;

    bool _thrown;
    Rigidbody _rigidbody;
    GameObject _ghost;
    Renderer _renderer;
    Vector3 _initialpos;
    //UnityEngine.XR.InputDevice controller;
    Vector3 _throwPos;
    Vector3 _throwVelocity;
    float _time;

    private bool _nextSphereInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        _thrown = false;
        _rigidbody = GetComponent<Rigidbody>();
        _ghost = transform.Find("Ghost sphere").gameObject;
        _renderer = GetComponent<Renderer>();
        _initialpos = transform.position;
        /*var gameControllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller, gameControllers);
        controller = gameControllers[0];*/
        //Debug.LogWarning(gameControllers.Count);
        /*UnityEngine.XR.InputDevices.GetDevices(gameControllers);
        foreach (var c in gameControllers)
        {
            Debug.LogWarning(c.name + c.characteristics);
        }*/

        //_formula = KeyboardReader.Shunting_yard_algorithm("p+v*t-g/2*t^2"); //todo get this from keyboard instead
    
    }

    // Update is called once per frame
    void Update()
    {
        /*bool triggerValue;
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out triggerValue) && triggerValue)
        {
            _rigidbody.position = _initialpos;
            _rigidbody.velocity = new Vector3(0, 0, 0);
            TakeControl();
        }*/

        
    }

    private void FixedUpdate()
    {
        if (_thrown)
        {
            if (_time==0)
            {
                _throwPos = _rigidbody.position;
                _throwVelocity = _rigidbody.velocity;
                _rigidbody.isKinematic = true;
                _ghost.transform.parent = null;
                _ghost.SetActive(true);
                
            }

            if (_time > 15)
            {
                _sphereRespawnManager.DestroySphere(gameObject);
            }

            if (Formula != null)
            {
                Vector3 newPos = KeyboardReader.getVectorFromFormula(Formula, _throwPos, _throwVelocity, _time);
                _rigidbody.MovePosition(newPos);
                Vector3 truePos = _throwPos + _time * _throwVelocity + 0.5f * _time * _time * KeyboardReader.g;
                if (newPos == truePos)
                {
                    _ghost.GetComponent<MeshRenderer>().material = greenMaterial;
                }
                else
                {
                    _ghost.GetComponent<MeshRenderer>().material = redMaterial;
                }
                _ghost.GetComponent<Rigidbody>().MovePosition(truePos);
            }
            else
            {
                ResetPos();
            }
            _time += Time.deltaTime;
        }
    }

    public void TakeControl()
    {
        _thrown = false;
        _rigidbody.isKinematic = false;
        _ghost.transform.parent = transform;
        _ghost.SetActive(false);
    }

    public void HoverOver()
    {
        _renderer.material.EnableKeyword("_EMISSION");
    }

    public void HoverEnd()
    {
        _renderer.material.DisableKeyword("_EMISSION");
    }

    public void Throw()
    {
        //this line prevents a bug where the game thinks the object is kinematic
        _rigidbody.isKinematic = false;

        _thrown = true;
        _time = 0;

        if (!_nextSphereInstantiated)
        {
            _sphereRespawnManager.InstantiateNewSphere();
            _nextSphereInstantiated = true;
        }
        
    }

    public void ResetPos()
    {
        _rigidbody.position = _initialpos;
        _rigidbody.velocity = Vector3.zero;
        TakeControl();
    }

    public override void UpdateFormula(Queue<string> formula)
    {
        Formula = formula;
        if (_thrown)
        {
            _sphereRespawnManager.DestroySphere(gameObject);
        }
    }
}
