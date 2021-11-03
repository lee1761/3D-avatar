using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class Touchable : MonoBehaviour
{
	public bool trigger = false;


	public void pressed()
	{
		Debug.Log("pressed");

		trigger = true;
	}


	public void Unpressed()
	{
		Debug.Log("Unpressed");

		trigger = false;

	}


	public bool GetResult()
	{
		return trigger;
	}

}

