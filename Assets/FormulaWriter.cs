using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormulaWriter : MonoBehaviour
{

    private TouchScreenKeyboard _keyboard;
    private UnityEngine.UI.Text _parabolicaFormula;
    private UnityEngine.UI.Text _gravityFormula;
    private GrabInteraction _grabInteraction;
    private ConstantForce _constantForce;

    // Start is called before the first frame update
    void Start()
    {
        GameObject text = GameObject.Find("CRT Display").transform.Find("Text Display").Find("Canvas").Find("Editable Text").gameObject;
        _parabolicaFormula = text.GetComponent<UnityEngine.UI.Text>();

        text = GameObject.Find("CRT Display 2").transform.Find("Text Display").Find("Canvas").Find("Editable Text").gameObject;
        _gravityFormula = text.GetComponent<UnityEngine.UI.Text>();
        //Debug.LogWarning("find is" + (transform.Find("Modified sphere") == null).ToString());
        //Debug.LogWarning("gameobject is" + (transform.Find("Modified sphere").gameObject == null).ToString());
        //Debug.LogWarning("interaction is" + (transform.Find("Modified sphere").gameObject.GetComponent<GrabInteraction>() == null).ToString());
        _grabInteraction = GameObject.Find("Modified sphere").gameObject.GetComponent<GrabInteraction>();
        _constantForce = GameObject.Find("Sphere").gameObject.GetComponent<ConstantForce>();
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if (_keyboard != null)
        {
            UnityEngine.UI.Text formula;
            //string level = LevelManager.GetInstance().Level;
            string level = "parabola";
            if (level == "gravity")
            {
                formula = _gravityFormula;
            }
            else
            {
                formula = _parabolicaFormula;
            }
            formula.color = Color.green;
            formula.text = _keyboard.text;

            if (_keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                if (level == "gravity")
                {
                    if (_keyboard.text[0] != '[' || _keyboard.text[_keyboard.text.Length -1] != ']')
                    {
                        formula.color = Color.red;
                        formula.text = "Impossible to parse the formula :\nA vector must be in the format [a,b,c]";
                    }
                    else
                    {
                        string[] values = _keyboard.text.Replace("[", "").Replace("]", "").Split(',');
                        try
                        {
                            float a = float.Parse(values[0]);
                            float b = float.Parse(values[1]);
                            float c = float.Parse(values[2]);

                            _constantForce.force = new Vector3(a, b, c);
                        }
                        catch (FormatException e)
                        {
                            formula.color = Color.red;
                            formula.text = "Impossible to parse the formula :\nA vector must be in the format [a,b,c]";
                        }
                        finally
                        {
                            _keyboard = null;
                        }
                    }
                }
                else
                {
                    try
                    {
                        _grabInteraction.UpdateFormula(KeyboardReader.Shunting_yard_algorithm(_keyboard.text, false, false));
                    }
                    catch (IncorrectFormulaException e)
                    {
                        _parabolicaFormula.color = Color.red;
                        _parabolicaFormula.text = "Impossible to parse the formula :\n" + e.Message;
                    }
                    finally
                    {
                        _keyboard = null;
                    }
                }
                
                
            }

            
        }
    }

    public void OnInputFieldClick()
    {
        //_keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        Canvas c = GameObject.Find("Canvas").gameObject.GetComponent<Canvas>();
        c.enabled = true;
        Debug.LogWarning(c.enabled);
    }
}
