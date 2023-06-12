using UnityEngine;
using UnityEngine.UI;

public class Gravity2ConstantScript : MonoBehaviour
{
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private GameObject ghost;
    /*[SerializeField] private Slider sliderGx;
    [SerializeField] private Slider sliderGy;
    [SerializeField] private Slider sliderGz;*/
    [SerializeField] private Gravity2RespawnManager _respawnManager;


    private Vector3 _gravityInput;
    bool _thrown;
    Rigidbody _rigidbody;
    Renderer _renderer;
    Vector3 _initialpos;
    Vector3 _throwPos;
    Vector3 _throwVelocity;
    float _time;

    private bool _nextSphereInstantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        _thrown = false;
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _initialpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (_thrown)
        {
            if (_time == 0)
            {
                _throwPos = _rigidbody.position;
                _throwVelocity = _rigidbody.velocity;
                _rigidbody.isKinematic = true;
                ghost.transform.parent = null;
                ghost.SetActive(true);

            }

            if (_time > 15)
            {
                _respawnManager.DestroySphere(gameObject);
            }

          
            Vector3 newPos = _throwPos + _time * _throwVelocity + 0.5f * _time * _time * _gravityInput;
            _rigidbody.MovePosition(newPos);
            Vector3 truePos = _throwPos + _time * _throwVelocity + 0.5f * _time * _time * KeyboardReader.g;
            if (newPos == truePos)
            {
                ghost.GetComponent<MeshRenderer>().material = greenMaterial;
            }
            else
            {
                ghost.GetComponent<MeshRenderer>().material = redMaterial;
            }
            ghost.GetComponent<Rigidbody>().MovePosition(truePos);
            
            _time += Time.deltaTime;
        }
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
            _respawnManager.InstantiateNewSphere();
            _nextSphereInstantiated = true;
        }

    }

    public void TakeControl()
    {
        _thrown = false;
        _rigidbody.isKinematic = false;
        ghost.transform.parent = transform;
        ghost.SetActive(false);
    }

    /*public void ApplyValue()
    {
        _gravityInput = new Vector3(sliderGx.value*0.98f, sliderGy.value*0.98f, sliderGz.value*0.98f);
    }*/

    public void ResetPos()
    {
        _rigidbody.position = _initialpos;
        _rigidbody.velocity = Vector3.zero;
        TakeControl();
    }

    public void UpdateGravity(Vector3 g)
    {
        _gravityInput = g;
        if (_thrown)
        {
            _respawnManager.DestroySphere(gameObject);
        }
    }

}
