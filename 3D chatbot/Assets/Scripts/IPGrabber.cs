using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class IPGrabber : MonoBehaviour
{
    string ipAddress;
    public string ipCountry, ipCity, ipRegion;
    public AppManager appManager;
    void Start()
    {
        StartCoroutine("GrabIP");
        StartCoroutine("IPLocation");
    }


    public string getLocation()
    {
        return ipCity;
    }

    public string getRegion()
    {
        return ipRegion;
    }



    IEnumerator GrabIP()
    {
        var response = new UnityWebRequest("https://api.ipify.org")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        
        if (response.isNetworkError || response.isHttpError)
        {
            yield break;
        }
        
        yield return response.SendWebRequest();
        
        yield return ipAddress = response.downloadHandler.text;
        
    }
    IEnumerator IPLocation()
    {
        var response = new UnityWebRequest("https://geo.ipify.org/api/v1?apiKey=at_DkoV9KmkQuqsYnuFYdc0Hh3fZylRd&" +ipAddress +"=8.8.8.8")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return response.SendWebRequest();

        
        if(response.isNetworkError || response.isHttpError)
        {
            print("error");
            yield break;
        }
        

        JSONNode IPlocation = JSON.Parse(response.downloadHandler.text);
        yield return ipCity = IPlocation["location"]["city"];
        yield return ipRegion = IPlocation["location"]["region"];

        yield return ipCountry = IPlocation["location"]["country"];

        appManager.StartCoroutine("GetWeather");
    }

    
}
