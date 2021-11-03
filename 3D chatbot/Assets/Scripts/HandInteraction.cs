using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteraction : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "R_Palm" || other.gameObject.tag == "R_Palm")
        {
            Debug.Log("Palm palm");
        }
    }



    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "R_Palm" || other.gameObject.tag == "R_Palm")
        {
            Debug.Log("Palm palm");
        }
    }


    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "R_Palm" || other.gameObject.tag == "R_Palm")
        {
            Debug.Log("Palm palm");
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name == "R_Palm" || other.gameObject.tag == "R_Palm")
        {
            Debug.Log("Palm palm");
        }
    }


}
