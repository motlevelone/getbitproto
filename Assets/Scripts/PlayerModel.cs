using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {
    public PlayerColor playerColor;
    public Animator anim;
    public Material[] materials;
    public GameObject[] limbs;
	public GameObject[] bloods;
    public SkinnedMeshRenderer[] models;
    List<int> limbsIndex;
	float[] playerXPos = { -8.1f, -4.4f, -0.7f, 2.3f, 5.9f};
	int curPos;
	Vector3 newPos;
    void OnEnable()
    {
//		SetPosition ((int)playerColor);
        SetSwim();
        limbsIndex = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            limbsIndex.Add(i);
            limbs[i].SetActive(true);
			bloods [i].SetActive (false);
        }
        for (int i = 0; i < models.Length; i++)
        {
            models[i].material = materials[(int)playerColor];
        }
    }

	public void SetPosition(int pos) {
		curPos = pos;
		transform.localPosition = new Vector3 (playerXPos[curPos],0f,0f);
		newPos = transform.localPosition;
	}

	void Update() {
		if (!Vector3.Equals (transform.localPosition, newPos)) {
			transform.localPosition = Vector3.Lerp (transform.localPosition,newPos,0.1f);
		}
	}

	public void SetSwim()
    {
        anim.SetInteger("State",0);
		AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo (0);
		anim.Play(animState.fullPathHash,-1,Random.Range(0f,1f));
    }

    public void SetWin()
    {
        anim.SetInteger("State", 1);
    }

    public void GetBit()
    {
        anim.SetInteger("State",2);
        if (limbsIndex.Count > 0)
        {
            int randomLimbs = Random.Range(0, limbsIndex.Count);
            limbs[limbsIndex[randomLimbs]].SetActive(false);
			bloods[limbsIndex[randomLimbs]].SetActive(true);
            limbsIndex.RemoveAt(randomLimbs);
        }

    }

	public void Forward() {
		curPos = 0;
		transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y-2,transform.localPosition.z);
		newPos = new Vector3 (playerXPos[curPos],0f,0f);
		SetSwim ();
	}

	public void Backward() {
		if (curPos < 4) {
			curPos++;
			newPos = new Vector3 (playerXPos [curPos], 0f, 0f);
		}
	}
}
