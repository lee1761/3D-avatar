using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;


public class AppManager : MonoBehaviour
{
    public float appRefresh;
    private float timer;
    public Text currentWeatherText, tempText, cityNameText, currentTimeText;



    //public WeatherStates weatherController;
    public IPGrabber ip;
    private void Start()
    {
        timer = appRefresh;

    }

    public string GetTime()
    {
        return currentTimeText.text;
    }


 
    public string GetWeatherStatus()
    {
        return currentWeatherText.text;
    }

    public string GetTempStatus()
    {
        return tempText.text;
    }

    public IEnumerator GetWeather()
    {
        var weatherAPI = new UnityWebRequest("https://api.openweathermap.org/data/2.5/weather?q=" + ip.ipCity + "," + ip.ipCountry + "&units=metric&APPID=b65a8aa8e00d2491b360a15824da0856")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };
        yield return weatherAPI.SendWebRequest();

        if (weatherAPI.isNetworkError || weatherAPI.isHttpError)
        {
            print("Fail getting data");
            yield break;
        }

        JSONNode weatherInfo = JSON.Parse(weatherAPI.downloadHandler.text);

        //currentWeatherText.text = "Current weather: " + weatherInfo["weather"][0]["description"];
        //tempText.text = "Current temperature: " + Mathf.Floor(weatherInfo["main"][0]) + "°C";

        currentWeatherText.text = weatherInfo["weather"][0]["description"];
        tempText.text = Mathf.Floor(weatherInfo["main"][0]) + "°C";

        cityNameText.text = weatherInfo["name"];


        if (currentWeatherText.text.Contains("cloud"))
        {
            currentWeatherText.text = "Cloudy";
        }

        if (currentWeatherText.text.Contains("clear"))
        {
            currentWeatherText.text = "Clear";
        }

        if (currentWeatherText.text.Contains("rain"))
        {
            currentWeatherText.text = "Rainy";
        }


        if (DateTime.Now.Hour > 12)
        {
            currentTimeText.text = DateTime.Now.ToString((DateTime.Now.Hour - 12) + ":" + DateTime.Now.Minute);

            if (DateTime.Now.Minute < 10)
            {
                currentTimeText.text = DateTime.Now.ToString((DateTime.Now.Hour - 12) + ": 0" + DateTime.Now.Minute);
            }

            timer -= Time.deltaTime;

        }

        if (DateTime.Now.Hour <= 12)
        {
            currentTimeText.text = DateTime.Now.ToString((DateTime.Now.Hour) + ":" + DateTime.Now.Minute);

            if (DateTime.Now.Minute < 10)
            {
                currentTimeText.text = DateTime.Now.ToString((DateTime.Now.Hour) + ": 0" + DateTime.Now.Minute);


            }
            timer -= Time.deltaTime;

        }



        /*
                if (weatherInfo["weather"][0]["icon"] == "01d")
                {
                    weatherController.ClearDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "01n")
                {
                    weatherController.ClearNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "02d")
                {
                    weatherController.CloudCover();
                    weatherController.ClearDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "02n")
                {
                    weatherController.CloudCover();
                    weatherController.ClearNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "03d")
                {
                    weatherController.CloudsDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "03n")
                {
                    weatherController.CloudsNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "10d")
                {
                    weatherController.RainDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "10n")
                {
                    weatherController.RainNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "09n")
                {
                    weatherController.CloudCover();
                    weatherController.RainNightLight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "09d")
                {
                    weatherController.CloudCover();
                    weatherController.RainDayLight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "50d")
                {

                    weatherController.MistDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "50n")
                {
                    weatherController.MistNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "13d")
                {
                    weatherController.SnowDay();
                }
                else if (weatherInfo["weather"][0]["icon"] == "13n")
                {
                    weatherController.SnowNight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "02d")
                {
                    weatherController.CloudsDayLight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "02n")
                {
                    weatherController.CloudsNightLight();
                }
                else if (weatherInfo["weather"][0]["icon"] == "04d")
                {
                    weatherController.CloudsDayBroken();
                }
                else if (weatherInfo["weather"][0]["icon"] == "04n")
                {
                    weatherController.CloudsNightBroken();
                }


                print(weatherInfo["weather"][0]["description"]);
                print(weatherInfo["weather"][0]["icon"]);
                print(weatherInfo["weather"][0]["main"]);
                */


    }

    private void Update()
    {

        

        //currentTimeText.text = DateTime.Now.ToString(DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
        //timer -= Time.deltaTime;


        if (timer <= 0)
        {
            StopCoroutine("GetWeather");
            StartCoroutine("GetWeather");
            print("App Refresh");

            timer = appRefresh;
        }


    }


}
