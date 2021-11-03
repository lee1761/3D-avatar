using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportMe : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverBegin()
    {
        Debug.Log("Hover Begin");
    }

    public void HoverEnd()
    {
        Debug.Log("Hover End");
    }

    public void HoverStay()
    {
        Debug.Log("Hover is staying");
    }
}
