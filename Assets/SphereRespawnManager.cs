using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRespawnManager : FormulaControlled
{
    [SerializeField] private GameObject _firstSphere;

    private List<GameObject> _spheres;
    private GameObject _nextSphere;

    private void Awake()
    {
        _spheres = new List<GameObject>();
        _spheres.Add(_firstSphere);

        _nextSphere = Instantiate(_firstSphere);
        _nextSphere.SetActive(false);
        _spheres.Add(_nextSphere);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void UpdateFormula(Queue<string> formula)
    {
        Formula = formula;
        for (int i = _spheres.Count - 1; i>=0; i --)
        {
            _spheres[i].GetComponent<GrabInteraction>().UpdateFormula(Formula);
        }
    }


    public void InstantiateNewSphere()
    {
        _nextSphere.SetActive(true);
        _nextSphere = Instantiate(_nextSphere);
        _nextSphere.SetActive(false);
        _nextSphere.GetComponent<GrabInteraction>().UpdateFormula(Formula);
        _spheres.Add(_nextSphere);
    }

    public void DestroySphere(GameObject s)
    {
        _spheres.Remove(s);
        Destroy(s);
    }
}
