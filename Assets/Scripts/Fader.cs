using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FaderState
{
	Full,
	None,
	FadeIn,
	FadeOut
}


public class Fader : MonoBehaviour {
	public float fadeInSpeed;
	public float fadeOutSpeed;

	public Image faderImage;

	FaderState faderState;

	public delegate void FadeInFinished();
	public event FadeInFinished OnFadeInFinished;
	public delegate void FadeOutFinished();
	public event FadeOutFinished OnFadeOutFinished;

	public void SetFaderActive(bool faderFull)
	{
		if (faderFull) {
			gameObject.SetActive (true);
			faderState = FaderState.Full;
			faderImage.color = Color.black;
		} else {
			gameObject.SetActive (false);
			faderState = FaderState.None;
			faderImage.color = Color.clear;
		}
	}

	public void FadeIn()
	{
		if ((faderState != FaderState.FadeIn) && (faderState != FaderState.FadeOut)) {
			SetFaderActive (true);
			faderState = FaderState.FadeIn;
			StartCoroutine (AnimateFade(fadeInSpeed));
		}
	}
	public void FadeOut()
	{
//		Debug.Log ("FaderState: "+ faderState);
		if ((faderState != FaderState.FadeIn) && (faderState != FaderState.FadeOut)) {
			gameObject.SetActive (true);
			faderState = FaderState.FadeOut;
			faderImage.color = Color.clear;
			StartCoroutine (AnimateFade(fadeOutSpeed));
		}
	}

	IEnumerator AnimateFade(float fadeSpeed)
	{
		Color targetColor = faderState == FaderState.FadeIn ? Color.clear : Color.black;
		float timer = 0f;
		while(timer<0.1f)
		{
			timer += (Time.deltaTime * fadeSpeed);
			faderImage.color = Color.Lerp(faderImage.color,targetColor,timer);
			yield return null;
		}
		if (faderState == FaderState.FadeIn) {
			SetFaderActive (false);
			if (OnFadeInFinished != null)
				OnFadeInFinished ();
		} else {
			SetFaderActive (true);
			if (OnFadeOutFinished != null)
				OnFadeOutFinished ();
		}
	}

}
