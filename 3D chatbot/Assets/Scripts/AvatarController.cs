using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static SpeechToText;


public class AvatarController : MonoBehaviour
{
    public Transform Hand;
    bool attemptingShake = false;
    public Transform UserHand;

    private Animator anim1;
    bool hDown;
    bool isHello;

    [SerializeField]
    private SpeechToText stt; // IBM Watson Speech to Text gameobject


    [SerializeField]
    private TextToSpeech tts; // IBM Watson Speech to Text gameobject


    void Awake()
    {
        anim1 = GetComponentInChildren<Animator>();
    }

    static void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hDown = Input.GetButtonDown("Hello");

        /*
        if (hDown)
        {
            anim1.SetTrigger("doHello");

        }
        */
        

        if (stt.GetResult() == "hi how are you ")
        {

            anim1.SetTrigger("doHello");
            //anim1.Play("Hello",-1,0f);
            stt.SetText();

        }



        if (stt.GetResult() == "nice to meet you " || hDown)
        {

            anim1.SetTrigger("doNice");
            //anim1.Play("Hello",-1,0f);
            //attemptingShake = true;
            

            stt.SetText();

        }



        if (stt.GetResult() == "hi 5 " || stt.GetResult() == "High-Five! " || hDown)
        {

            anim1.SetTrigger("doHighfive");
            //anim1.Play("Hello",-1,0f);
            //attemptingShake = true;


            stt.SetText();

        }




        if (tts.GetResult() == "Don't touch me")
        {

            //anim1.SetTrigger("doHello");
            anim1.SetTrigger("doUnTouchable");

           
            tts.SetText();

        }


        
        if (attemptingShake)
        {
            Hand.position = Vector3.Lerp(Hand.position, UserHand.position, Time.deltaTime/10f);

            if (UserHand.position == Hand.position)
            {
                attemptingShake = false;
            }
        }
        


    }

}
