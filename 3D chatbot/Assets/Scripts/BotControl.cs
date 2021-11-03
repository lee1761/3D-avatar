using IBM.Cloud.SDK.Utilities;
using UnityEngine;

public class BotControl : MonoBehaviour
{
    [SerializeField]
    private TextToSpeech tts; // IBM Watson Text to Speech gameobject
    [SerializeField]
    private SpeechToText stt; // IBM Watson Speech to Text gameobject
    [SerializeField]
    private SimpleBot chat; // IBM Watson Assistant

    private void Start()
    {
        Runnable.Run(chat.Welcome());
    }
    void Update()
    {
        /* Wait for audio clip of the output speech to finish before listening and converting
         * new input speech
         */
        if (stt.ServiceReady() && tts.ServiceReady() && chat.ServiceReady())
        {
            if (chat.GetStatus() == SimpleBot.ProcessingStatus.Processing|| !tts.IsFinished())
            {
                stt.Active = false;
                stt.StopRecording();
            }
            else 
            {
                stt.Active = true;
                stt.StartRecording();
            }
        }
    


        /*
         if (stt.GetStatus() == SpeechToText.ProcessingStatus.Processed && chat.ServiceReady())
        {
            // GetResult obtains the result of the speech to text conversion and changes the speech input status to Idle.
            Runnable.Run(chat.ProcessChat(stt.GetResult()));
        }
       
        if (chat.GetStatus() == SimpleBot.ProcessingStatus.Processed && tts.ServiceReady())
        {
            // GetResult obtains the chat response and adds it to the queue for conversion to speech audio.
            tts.AddTextToQueue(chat.GetResult());
        }
        */
    }
}



