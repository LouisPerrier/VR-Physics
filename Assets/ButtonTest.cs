using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonTest : MonoBehaviour
{

    private KeyboardEditor kb;
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        kb = GameObject.Find("Canvas").gameObject.transform.Find("Panel").gameObject.GetComponent<KeyboardEditor>();
        text = transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        kb.OnInputReceived(text.text);
    }
}
