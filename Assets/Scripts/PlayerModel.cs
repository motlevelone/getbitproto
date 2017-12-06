using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour {
    public PlayerColor playerColor;
    public Animator anim;
    public Material[] materials;
    public GameObject[] limbs;
    public SkinnedMeshRenderer[] models;
    List<int> limbsIndex;

    void OnEnable()
    {
        SetSwim();
        limbsIndex = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            limbsIndex.Add(i);
            limbs[i].SetActive(true);
        }
        for (int i = 0; i < models.Length; i++)
        {
            models[i].material = materials[(int)playerColor];
        }
    }

    public void SetSwim()
    {
        anim.SetInteger("State",0);
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
            limbs[randomLimbs].SetActive(false);
            limbsIndex.Remove(randomLimbs);
        }

    }
}
