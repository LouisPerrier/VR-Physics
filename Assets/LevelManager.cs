using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private GameObject _character;

    //using controller buttons
    /*
    private bool buttonPressed = false;
    UnityEngine.XR.InputDevice controller;
    */

    //Using singleton pattern
    /*
    private static LevelManager instance;
    public string Level = "parabola";

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            var gameControllers = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller, gameControllers);
            controller = gameControllers[0];
        }
    }
    */

    public void Start()
    {
        
    }

    private void Update()
    {
        //using controller buttons
        /*
        bool triggerValue;
        if (controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out triggerValue))
        {
            if (triggerValue && !buttonPressed)
            {
                if (Level == "parabola")
                    SetLevel("gravity");
                else
                    SetLevel("parabola");
                buttonPressed = true;
            }
            else if (!triggerValue && buttonPressed)
                buttonPressed = false;
        }
        */
    }

    public void SetLevel(string lv)
    {
        if (lv == "gravity")
        {
            _character.transform.position = new Vector3(-10, 0, 0);
        }
        else if (lv == "parabola")
        {
            _character.transform.position = new Vector3(0, 0, 0);
        }
        else if (lv == "km")
        {
            _character.transform.position = new Vector3(20, 0, 0);
        }
        else if (lv == "oscillation")
        {
            _character.transform.position = new Vector3(30, 0, 0);
        }
            //using singleton pattern
            //Level = lv;
        }

        //using singleton pattern
        /*
        public static LevelManager GetInstance()
        {
            return instance;
        }
        */
    }
