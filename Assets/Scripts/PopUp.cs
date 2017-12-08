using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopUpType {
	READY,
	SET,
	GO,
	DUPLICATES,
	WIN,
	LOSE
}

public class PopUp : MonoBehaviour {
	public GameObject[] popUpType;
    public Animator anim;
    public Button actionButton;

	public void ShowPopUp(PopUpType type) {
        gameObject.SetActive(true);
        anim.SetTrigger("Start");
		for (int i = 0; i < popUpType.Length; i++) {
			popUpType [i].SetActive (false);
		}
		popUpType [(int)type].SetActive (true);
    }

    public void HidePopUp() {
        gameObject.SetActive(false);
    }
}
