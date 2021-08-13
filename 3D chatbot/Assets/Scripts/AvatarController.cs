using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{

	Animator anim;
	bool hDown;
    bool jDown;
    bool isHello;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Awake()
    {
    	anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        jDown = Input.GetButtonDown("HelloHello");
        if(jDown)
        {
            anim.SetTrigger("doHello");
        }
    }


}

