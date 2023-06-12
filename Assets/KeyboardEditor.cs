using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

public class KeyboardEditor : MonoBehaviour
{
    private List<string> _list;
    private int _caretPosition = 0;
    private int _charactersBeforeCaret = 0;
    private float _elapsed = 0;
    private bool _started = false;
    private Vector3 _caretDefaultpos;
    [SerializeField] private TMP_InputField _text;
    [SerializeField] private FormulaControlled _formulaControlled;
    [SerializeField] private GameObject _errorPanel;
    [SerializeField] private GameObject _keyboardPanel;
    [SerializeField] private TMP_Text _tmp_errorText;
    [SerializeField] private bool _gravityLevel;
    [SerializeField] private GameObject _caret;

    // Start is called before the first frame update
    void Start()
    { 
        if (!_started)
        {
            _list = new List<string>();
            _caretDefaultpos = _caret.transform.localPosition;

            if (_gravityLevel)
            {
                OnInputReceived("p<sub>0</sub>");
                OnInputReceived("+");
                OnInputReceived("v<sub>0</sub>");
                OnInputReceived("×");
                OnInputReceived("t");
            }
            else
            {
                OnInputReceived("0");
            }

            OnInputReceived("Enter");
            _started = true;
        }
        





        //before using SerializeFields
        /*
        _text = transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>();
        _grabInteraction = GameObject.Find("Modified sphere").gameObject.GetComponent<GrabInteraction>();
        */

        //test for making cursor appear
        /*
        text.Select();
        text.ActivateInputField();
        text.caretPosition = 0;
        text.GetType().GetField("m_AllowInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(text, true);
        text.GetType().InvokeMember("SetCaretVisible", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, text, null);
        */

        /*if (_gravityLevel)
        {
            _text.GetType().GetField("m_AllowInput", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(_text, true);
            _text.GetType().InvokeMember("SetCaretVisible", BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Instance, null, _text, null);

        }*/
    }

    // Update is called once per frame
    void Update()
    {
         _elapsed += Time.deltaTime * 2;
         if (Mathf.FloorToInt(_elapsed) % 2 == 0)
         {
             _caret.SetActive(true);
         }
         else
         {
             _caret.SetActive(false);
         }
         //Debug.LogWarning(_caretPosition);
         TMP_TextInfo textInfo = _text.textComponent.textInfo;
         if (_charactersBeforeCaret > 0)
         {
         TMP_CharacterInfo caretInfo = textInfo.characterInfo[_charactersBeforeCaret - 1];
         Vector2 caretScreenPosition = caretInfo.topRight;
                //Debug.LogWarning(caretScreenPosition.x);
                /*Vector3 caretLocalPosition = _caret.transform.parent.InverseTransformPoint(caretScreenPosition);
                _caret.transform.localPosition = caretLocalPosition;*/
         _caret.transform.localPosition = new Vector3(caretScreenPosition.x, 15, 0);
                //Debug.LogWarning(_caret.transform.localPosition);
                //Vector3 caretLocalPosition = caretObject.transform.parent.InverseTransformPoint(caretScreenPosition);
                //Debug.LogWarning(_caret.transform.position.x);
                //InvokeRepeating("ToggleCaretVisibility", 5f, 5f);
        }
        else
        {
            _caret.transform.localPosition = _caretDefaultpos;
        }
        

    }

    public void LeftArrowPressed()
    {

        if (_gravityLevel)
            Debug.LogWarning("- " + _caretPosition);

        if (_gravityLevel)
            Debug.LogWarning("before caret : " + _charactersBeforeCaret);

        if (_caretPosition > 0)
        {
            _caretPosition -= 1;
            string s = _list.ElementAt(_caretPosition);
            if (s.Length > 1 && s[1] == '<')
            {
                _charactersBeforeCaret -= 2;
            }
            else
            {
                _charactersBeforeCaret -= s.Length;
            }
            if (_gravityLevel)
                Debug.LogWarning("after before caret : " + _charactersBeforeCaret);
        }
    }

    public void RightArrowPressed()
    {
        if (_caretPosition < _list.Count)
        {
            string s = _list.ElementAt(_caretPosition);
            if (s.Length > 1 && s[1] == '<')
            {
                _charactersBeforeCaret += 2;
            }
            else
            {
                _charactersBeforeCaret += s.Length;
            }
            _caretPosition += 1;
        }
    }

    public void OnInputReceived(string s)
    {
        if(s =="Enter")
        {
            try
            {
                //using Tokens instead of strings
                /*List<Token> tokens = new List<Token>();
                foreach(string t in stack)
                {
                    switch (t)
                    {
                        case "0": tokens.Insert(0,new Number(0)); break;
                        case "1": tokens.Insert(0, new Number(1)); break;
                        case "2": tokens.Insert(0, new Number(2)); break;
                        case "3": tokens.Insert(0, new Number(3)); break;
                        case "4": tokens.Insert(0, new Number(4)); break;
                        case "5": tokens.Insert(0, new Number(5)); break;
                        case "6": tokens.Insert(0, new Number(6)); break;
                        case "7": tokens.Insert(0, new Number(7)); break;
                        case "8": tokens.Insert(0, new Number(8)); break;
                        case "9": tokens.Insert(0, new Number(9)); break;

                        case "(": tokens.Insert(0, new OpenParenthesis()); break;
                        case ")": tokens.Insert(0, new CloseParenthesis()); break;

                        case "+": tokens.Insert(0, new Plus()); break;
                        case "-": tokens.Insert(0, new Minus()); break;
                        case "×": tokens.Insert(0, new Times()); break;
                        case "/": tokens.Insert(0, new DividedBy()); break;
                        case "^": tokens.Insert(0, new Power()); break;

                        default: tokens.Insert(0, new Variable(t[0])); break;
                    }
                }

                _grabInteraction.formula = KeyboardReader.Shunting_yard_algorithm(tokens);*/
                if (_gravityLevel)
                    _formulaControlled.UpdateFormula(KeyboardReader.Shunting_yard_algorithm(_list.Aggregate("", (acc, next) => acc + next[0]), true, false));
                else
                    _formulaControlled.UpdateFormula(KeyboardReader.Shunting_yard_algorithm(_list.Aggregate("", (acc, next) => acc + next[0]), false, true));
            }
            catch (IncorrectFormulaException e)
            {
                _keyboardPanel.SetActive(false);
                _errorPanel.SetActive(true);
                _tmp_errorText.text = "Parsing error :\n" + e.Message;
            }
        }
        else if (s== "←")
        {
            if (_list.Count > 0)
            {
                string toRemove = _list.ElementAt(_caretPosition-1);
                _list.RemoveAt(_caretPosition-1);
                _caretPosition -= 1;
                if (toRemove.Length > 1 && toRemove[1] == '<')
                {
                    _charactersBeforeCaret -= 2;
                }
                else
                {
                    _charactersBeforeCaret -= toRemove.Length;
                }
                _text.text = _list.Aggregate("", (acc, next) => acc + next);
            }
           
        }
        else
        {
            _list.Insert(_caretPosition, s);
            _caretPosition += 1;
            if (s.Length > 1 && s[1] == '<')
            {
                _charactersBeforeCaret += 2;
            }
            else
            {
                _charactersBeforeCaret += s.Length;
            }
            
            if (s=="sin" || s== "√")
            {
                _list.Insert(_caretPosition, "(");
                _caretPosition += 1;
                _charactersBeforeCaret += 1;
            }
            Debug.LogWarning(_caretPosition);
            Debug.LogWarning(_charactersBeforeCaret);
            foreach (string test in _list)
                Debug.LogWarning(test);
           _text.text = _list.Aggregate("", (acc, next) => acc + next);
        }
    }
}
