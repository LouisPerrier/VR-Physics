using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GravityConstantScript : MonoBehaviour
{

    [SerializeField] private Slider _sliderX;
    [SerializeField] private Slider _sliderY;
    [SerializeField] private Slider _sliderZ;
    [SerializeField] private GameObject _ball;
    private Vector3 _initialPos;

    // Start is called before the first frame update
    void Start()
    {
        _initialPos = _ball.transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGravity()
    {
        _ball.GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        _ball.GetComponent<Rigidbody>().Sleep();
        _ball.GetComponent<ConstantForce>().force = _ball.GetComponent<Rigidbody>().mass * new Vector3(_sliderX.value, _sliderY.value, _sliderZ.value);
    }

    public void ResetLevel()
    {
        _ball.GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
        _ball.GetComponent<Rigidbody>().Sleep();
        _ball.transform.position = _initialPos;
        _sliderX.value = 0;
        _sliderY.value = 0;
        _sliderZ.value = 0;
    }
}
