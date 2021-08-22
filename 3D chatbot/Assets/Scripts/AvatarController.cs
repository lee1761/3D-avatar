using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SpeechToText;


public class AvatarController : MonoBehaviour
{

   private Animator anim;

    SpeechToText stt = new SpeechToText();
    string a;

	void Awake()
	{
    	anim = GetComponentInChildren<Animator>();
	}

    void Start()
    {
         a = stt.outText;
    }

    // Update is called once per frame
    void Update()
    {
        if(a.Equals("hi how are you "))
        {
        	Debug.Log("hooooh");
            //anim.SetTrigger("Hello");
        }
    }
   
}
