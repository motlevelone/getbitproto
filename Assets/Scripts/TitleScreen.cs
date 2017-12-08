using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {
	public Fader fader;
	public GameObject selectionScreen;
	public Transform startCamPos;

	void OnEnable () {
		fader.FadeIn ();
		Camera.main.transform.localRotation = startCamPos.localRotation;
	}

	public void ClickStart() {
		fader.FadeOut ();
		fader.OnFadeOutFinished += FadeFinished;
	}

	void FadeFinished() {
		fader.OnFadeOutFinished -= FadeFinished;
		gameObject.SetActive (false);
		selectionScreen.SetActive(true);
		fader.FadeIn ();
	}
}
