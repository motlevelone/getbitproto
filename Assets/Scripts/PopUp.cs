using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour {
    public Text popUpLabel;
    public Animator anim;
    public Button actionButton;

    public void ShowPopUp(string label, int size = 70) {
        gameObject.SetActive(true);
        anim.SetTrigger("Start");
        popUpLabel.fontSize = size;
        popUpLabel.text = label;
    }

    public void HidePopUp() {
        gameObject.SetActive(false);
    }
}
