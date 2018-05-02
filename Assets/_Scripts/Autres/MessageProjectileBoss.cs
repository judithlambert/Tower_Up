using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageProjectileBoss : MonoBehaviour {

    Text MessageText;
    const string MESSAGE = "Appuyer sur la touche X ou L pour lancer des projectiles";
	
	void Start ()
    {
        DataÉtage.PausePlay();
        transform.localPosition = Vector2.zero;
        MessageText = gameObject.GetComponentInChildren<Text>();
        MessageText.text = MESSAGE;
    }
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.enter))
    //}

    public void BtnOk()
    {
        DataÉtage.PausePlay();
        Destroy(ÉtageBoss.MessagePanel);
    }
}
