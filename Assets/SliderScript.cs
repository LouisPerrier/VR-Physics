using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    [SerializeField] private KMConstantScript _km;

    // Start is called before the first frame update
    void Start()
    {
        _sliderText.text = _sliderText.text.Split('=')[0] + "= " + _slider.value.ToString("0.0");
        _slider.onValueChanged.AddListener(v =>
        {
            _sliderText.text = _sliderText.text.Split('=')[0] + "= " + v.ToString("0.0");
            _km.ApplyForce();
        }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
