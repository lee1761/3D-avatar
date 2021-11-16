#pragma warning disable 0649

using System;
using System.Collections;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBot : MonoBehaviour
{
    [SerializeField]
    private Touchable touch;

    [SerializeField]
    private WatsonSettings settings;

    private AssistantService Assistant_service;

    private bool createSessionTested = false;

    // 2020/2/15 Changed to protected to be accessible in CommandBot.cs
    protected bool messageTested = false;
    private bool deleteSessionTested = false;
    private string sessionId;

    public string textResponse = String.Empty;

    [SerializeField]
    private InputField targetInputField;

    [SerializeField]
    private Text targetText;

    //Keep track of whether IBM Watson Assistant should process input or is
    //processing input to create a chat response.
    public enum ProcessingStatus { Process, Processing, Idle, Processed };
    private ProcessingStatus chatStatus;

    public enum InputFieldTrigger { onValueChanged, onEndEdit };

    [SerializeField]
    private InputField externalInputField;
    [SerializeField]
    private InputFieldTrigger externalTriggerType;

    private InputField inputField;

    [SerializeField]
    private GameObject targetGameObject;

    private void Start()
    {
       

        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
        chatStatus = ProcessingStatus.Idle;

       

        if (externalInputField != null)
        {
            if (externalTriggerType == InputFieldTrigger.onEndEdit)
            {
                externalInputField.onEndEdit.AddListener(delegate { Runnable.Run(ProcessChat(externalInputField.text)); });
            }
            else
            {
                externalInputField.onValueChanged.AddListener(delegate { Runnable.Run(ProcessChat(externalInputField.text)); });
            }
        }

        inputField = gameObject.AddComponent<InputField>();
        inputField.textComponent = gameObject.AddComponent<Text>();
        inputField.onValueChanged.AddListener(delegate { Runnable.Run(ProcessChat(inputField.text)); });
    }

    private void Update()
    {

       
        if (targetText != null && textResponse != null)
        {
            targetText.text = textResponse;
        }
    }

    public IEnumerator CreateService()
    {

        if (string.IsNullOrEmpty(settings.Assistant_apikey))
        {
            throw new IBMException("Please provide Watson Assistant IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        //            IamAuthenticator authenticator = new IamAuthenticator(apikey: Assistant_apikey, url: serviceUrl);
        IamAuthenticator authenticator = new IamAuthenticator(apikey: settings.Assistant_apikey);

        //  Wait for tokendata
        while (!authenticator.CanAuthenticate())
            yield return null;

        Assistant_service = new AssistantService(settings.versionDate, authenticator);
        if (!string.IsNullOrEmpty(settings.serviceUrl))
        {
            Assistant_service.SetServiceUrl(settings.serviceUrl);
        }

        Assistant_service.CreateSession(OnCreateSession, settings.assistantId);

        while (!createSessionTested)
        {
            yield return null;
        }
    }

    // Get the "welcome" chat reponse from IBM Watson Assistant
    public IEnumerator Welcome()
    {
        Debug.Log("Welcome");
        // Set chat processing status to "Processing"
        chatStatus = ProcessingStatus.Processing;

        while (!createSessionTested)
        {
            yield return null;
        }

        Assistant_service.Message(OnMessage, settings.assistantId, sessionId);
        while (!messageTested)
        {
            messageTested = false;
            yield return null;
        }

        if (!String.IsNullOrEmpty(textResponse))
        {
            // Set status to show chat processing has finished 
            chatStatus = ProcessingStatus.Processed;
        }

    }

    public void GetChatResponse(string chatInput)
    {
        StartCoroutine(ProcessChat(chatInput));
    }

    public IEnumerator ProcessChat(string chatInput)
    {
        Debug.Log("Processchat: " + chatInput);

        // Set status to show that the chat input is being processed.
        chatStatus = ProcessingStatus.Processing;

        if (Assistant_service == null)
        {
            yield return null;
        }
        if (String.IsNullOrEmpty(chatInput))
        {
            yield return null;
        }

        messageTested = false;
        var inputMessage = new MessageInput()
        {
            Text = chatInput,
            Options = new MessageInputOptions()
            {
                ReturnContext = true
            }
        };

        Assistant_service.Message(OnMessage, settings.assistantId, sessionId, input: inputMessage);

        while (!messageTested)
        {
            messageTested = false;
            yield return null;
        }

        if (!String.IsNullOrEmpty(textResponse))
        {
            // Set status to show chat processing has finished.
            chatStatus = ProcessingStatus.Processed;
        }
    }

    private void OnDeleteSession(DetailedResponse<object> response, IBMError error)
    {
        deleteSessionTested = true;
    }

    // 2020/2/15 - changed to protected virtual to allow for inheritance.
    // This is where the returned chat response is set to send as output
    protected virtual void OnMessage(DetailedResponse<MessageResponse> response, IBMError error)
    {
        Debug.Log("response = " + response.Result.ToString());

        if (response == null ||
            response.Result == null ||
            response.Result.Output == null ||
            response.Result.Output.Generic == null ||
            response.Result.Output.Generic.Count < 1)
        {
            textResponse = "I don't know how to respond to that.";
        }

       
        else
        {
            textResponse = response.Result.Output.Generic[0].Text.ToString();
        }


        if (targetInputField != null)
        {
            targetInputField.text = textResponse;
        }

        // Check if the target GameObject has an InputField then place text into it.
        if (targetGameObject != null)
        {
            InputField target = targetGameObject.GetComponent<InputField>();
            if (target != null)
            {
                target.text = textResponse;
            }
        }

        Debug.Log(textResponse);
        messageTested = true;
    }

    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("SimpleBot.OnCreateSession()", "Session: {0}", response.Result.SessionId);
        sessionId = response.Result.SessionId;
        createSessionTested = true;
    }

    public void SetChatStatus(ProcessingStatus status)
    {
        chatStatus = status;
    }

    public ProcessingStatus GetStatus()
    {
        return chatStatus;
    }

    public bool ServiceReady()
    {
        //return createSessionTested;
        return Assistant_service != null;
    }

    public string GetResult()
    {
        chatStatus = ProcessingStatus.Idle;
        return textResponse;
    }

}

