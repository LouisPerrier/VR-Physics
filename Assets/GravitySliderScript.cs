using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravitySliderScript : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderText;
    [SerializeField] private Gravity2RespawnManager _respawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _sliderText.text = _sliderText.text.Split('=')[0] + "= " + (_slider.value*0.98).ToString("0.0");
        _slider.onValueChanged.AddListener(v => {
            _sliderText.text = _sliderText.text.Split('=')[0] + "= " + (v * 0.98).ToString("0.0");
            _respawnManager.UpdateGravity();
            });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
