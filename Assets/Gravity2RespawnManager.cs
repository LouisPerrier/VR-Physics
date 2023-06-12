using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gravity2RespawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _firstSphere;
    [SerializeField] private Slider sliderGx;
    [SerializeField] private Slider sliderGy;
    [SerializeField] private Slider sliderGz;

    private List<GameObject> _spheres;
    private GameObject _nextSphere;

    private Vector3 _gravityInput;

    // Start is called before the first frame update
    void Start()
    {
        _spheres = new List<GameObject>();
        _spheres.Add(_firstSphere);

        _nextSphere = Instantiate(_firstSphere);
        _nextSphere.SetActive(false);
        _spheres.Add(_nextSphere);

        _gravityInput = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateGravity()
    {
        _gravityInput = new Vector3(sliderGx.value * 0.98f, sliderGy.value * 0.98f, sliderGz.value * 0.98f);

        for (int i = _spheres.Count - 1; i >= 0; i--)
        {
            _spheres[i].GetComponent<Gravity2ConstantScript>().UpdateGravity(_gravityInput);
        }
    }

    public void InstantiateNewSphere()
    {
        _nextSphere.SetActive(true);
        _nextSphere = Instantiate(_nextSphere);
        _nextSphere.SetActive(false);
        _nextSphere.GetComponent<Gravity2ConstantScript>().UpdateGravity(_gravityInput);
        _spheres.Add(_nextSphere);
    }

    public void DestroySphere(GameObject s)
    {
        _spheres.Remove(s);
        Destroy(s);
    }
}
