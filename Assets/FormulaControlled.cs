using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FormulaControlled : MonoBehaviour
{
    protected Queue<string> Formula;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void UpdateFormula(Queue<string> formula);
}
