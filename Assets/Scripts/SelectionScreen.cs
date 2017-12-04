using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionScreen : MonoBehaviour {
    public GameManager manager;
    public RectTransform selectIcon;
    int selIdx;
    Vector2 targetSelect;

    void OnEnable()
    {
        selectIcon.anchoredPosition = targetSelect = new Vector2(600,0);        
        selIdx = -2;

    }

    public void MoveSelection(int delta)
    {
        selIdx += delta;
        selIdx = selIdx < -2 ? -2 : selIdx;
        selIdx = selIdx > 2 ? 2 : selIdx;
        targetSelect = new Vector2(selIdx * -300,0);
    }

    void Update()
    {
        if (!selectIcon.anchoredPosition.Equals(targetSelect))
        {
            selectIcon.anchoredPosition = Vector2.Lerp(selectIcon.anchoredPosition,targetSelect,0.1f);
        }
    }

    public void ClickStart()
    {
        gameObject.SetActive(false);
        manager.InitGame(selIdx+2);
    }
}
