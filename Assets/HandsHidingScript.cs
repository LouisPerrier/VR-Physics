using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandsHidingScript : MonoBehaviour
{

    [SerializeField] private GameObject _hands;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide()
    {
        Debug.LogWarning("here!!!");
        _hands.SetActive(false);
    }

    public void Show()
    {
        _hands.SetActive(true);
    }
}
