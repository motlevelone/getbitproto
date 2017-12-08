using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkModel : MonoBehaviour {
    public Animator anim;
	public GameObject sharkBlood;
	float[] sharkXPos = { -6.35f,-2.44f,1.45f,4.94f,8.39f,11.8f};
	float[] sharkXEatPos = { -8f,-4.54f,-0.96f,2.47f,5.74f,9.05f};
	int curPlayer;
	Vector3 newPos;

    void OnEnable()
    {
        SetSwim(5);
    }

	public void SetSwim(int _curPlayer)
    {
		sharkBlood.SetActive (false);
		curPlayer = _curPlayer;
        anim.SetInteger("State",0);
		transform.localPosition = new Vector3 (sharkXPos[curPlayer],-0.39f,0f);
		newPos = transform.localPosition;
    }

    public void SetEat()
    {
        anim.SetInteger("State", 1);
		sharkBlood.SetActive (true);
		transform.localPosition = new Vector3 (sharkXEatPos[curPlayer],0.7f,0f);
		newPos = transform.localPosition;
    }

	public void GoAway() {
		transform.localPosition = new Vector3 (sharkXEatPos[curPlayer],-4.43f,0f);
		newPos = transform.localPosition;
	}

	void Update() {
		if (!Vector3.Equals (transform.localPosition, newPos)) {
			transform.localPosition = Vector3.Lerp (transform.localPosition,newPos,0.1f);
		}
	}

}
