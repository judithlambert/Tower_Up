using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine;

public class UI : MonoBehaviour
{
    Text temps;
    char[] trim = new char[] { '.' };

	void Awake ()
    {
        temps = GetComponentInChildren<Text>();
	}
	
	void Update ()
    {
        temps.text = ((int)(Time.time / 60)).ToString() + ":" + ((int)(Time.time % 60)).ToString("00") + ":" + Time.time.ToString().Split(trim).Last();
	}
}
