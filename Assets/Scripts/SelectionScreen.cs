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
        selectIcon.anchoredPosition = targetSelect = new Vector2(1350,0);        
        selIdx = 0;

    }

    public void MoveSelection(int delta)
    {
        selIdx += delta;
        selIdx = selIdx < 0 ? 0 : selIdx;
        selIdx = selIdx > 9 ? 9 : selIdx;
		targetSelect = new Vector2(1350+ (selIdx * -300),0);
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
        manager.InitGame(selIdx);
    }
}
