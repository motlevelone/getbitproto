using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerColor
{
    RED01,
	RED02,
    BLUE01,
	BLUE02,
    GREEN01,
	GREEN02,
    ORANGE01,
	ORANGE02,
    WHITE01,
	WHITE02
}

[System.Serializable]
public struct CardSprites {
	public Sprite[] cards;
}

public class GameManager : MonoBehaviour {
    public Fader fader;
    public GameObject titleScreen;
	public Transform gameCamPos;
    public PopUp popup;
    public Transform allPlayer;
    public Transform graveyard;
    public PlayerStats[] playerStats;
	public SharkModel sharkModel;
    public float selectTime;
    public GameObject timer;
    public Slider timeSlider;
    int player;
	int playerVariant;
    public Image[] hands;
	public CardSprites[] cardSprite;
    public Animator handSelectAnim;
    public Image handSelect;
    public GameObject arrow;
    public Animator crunchAnim;

    bool gameOver;
    int curSelect;

    public void InitGame(int playerIdx)
    {
        player = playerIdx / 2;
		playerVariant = playerIdx;
        gameOver = false;
        gameObject.SetActive(true);
        arrow.SetActive(false);
        crunchAnim.gameObject.SetActive(false);
        popup.actionButton.onClick.RemoveAllListeners();
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].gameObject.SetActive(playerStats[player].IsCardReady(i));
			hands [i].sprite = cardSprite [player].cards [i];
        }
		StartCoroutine (AnimateCam());
        StartCoroutine(ReadyGame());
    }

	IEnumerator AnimateCam() {
		while (!Quaternion.Equals(Camera.main.transform.localRotation,gameCamPos.localRotation)) {
			Camera.main.transform.localRotation = Quaternion.Lerp (Camera.main.transform.localRotation,gameCamPos.localRotation,0.02f);
			yield return null;
		}
	}


    IEnumerator ReadyGame()
    {
        List<int> playerList = new List<int>();
        for (int i = 0; i < playerStats.Length; i++)
        {
            playerStats[i].transform.SetParent(allPlayer);
			playerStats[i].InitPlayer(i==player,i==player ? playerVariant : (i*2)+Random.Range(0,2));
            int idx = i;
            playerList.Add(idx);
        }
        for (int i = 0; i < playerStats.Length; i++)
        {
            int newIdx = Random.Range(0,playerList.Count);
			playerStats[i].transform.SetSiblingIndex(playerList[newIdx]);
			playerStats [i].playerModel.SetPosition (playerList[newIdx]);
            playerList.RemoveAt(newIdx);

        }
		sharkModel.SetSwim (allPlayer.childCount);
        EnableCards(false);
        popup.ShowPopUp(PopUpType.READY);
        yield return new WaitForSeconds(1f);
		popup.ShowPopUp(PopUpType.SET);
        yield return new WaitForSeconds(1f);
		popup.ShowPopUp(PopUpType.GO);
        yield return new WaitForSeconds(1f);
        popup.HidePopUp();

        NextRound();
    }

    void NextRound()
    {
		sharkModel.SetSwim (allPlayer.childCount);
        if (!gameOver)
        {
            StartCoroutine(SelectCards());
        }
        else
        {
            GameEnd();
        }
    }

    IEnumerator SelectCards()
    {
        timer.SetActive(true);
        timeSlider.maxValue = selectTime;
        timeSlider.value = selectTime;
        handSelectAnim.gameObject.SetActive(false);
        curSelect = -1;
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].gameObject.SetActive(playerStats[player].IsCardReady(i));
        }
        EnableCards(true);
        while (timeSlider.value > 0)
        {
            timeSlider.value -= Time.deltaTime;
            yield return null;
        }
        timer.SetActive(false);
        EnableCards(false);
        handSelectAnim.gameObject.SetActive(false);

        AllPlayersSelect();
    }

    public void ClickCard(int selection) {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].gameObject.SetActive(playerStats[player].IsCardReady(i));
        }
        hands[selection].gameObject.SetActive(false);

        handSelectAnim.SetTrigger("Start");
        handSelectAnim.gameObject.SetActive(true);
        handSelect.sprite = hands[selection].sprite;
        curSelect = selection;
    }

    void EnableCards(bool enabled)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i].GetComponent<Button>().interactable = enabled;
        }
    }

    void AllPlayersSelect()
    {
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].IsAlive())
                playerStats[i].SelectCards((i==player) ? curSelect : -1);
        }

        StartCoroutine(RemoveDuplicates());
    }

    IEnumerator RemoveDuplicates()
    {
        bool aDuplicates = false;
        for (int i = 0; i < playerStats.Length; i++)
        {
            for (int j = i + 1; j < playerStats.Length; j++)
            {
                if ((playerStats[i].GetSelected() == playerStats[j].GetSelected()) && (playerStats[i].IsAlive()) && (playerStats[j].IsAlive()))
                {
                    playerStats[i].DuplicateSelected();
                    playerStats[j].DuplicateSelected();
                    aDuplicates = true;
                }
            }
        }
        yield return null;
        if (aDuplicates)
        {
			popup.ShowPopUp(PopUpType.DUPLICATES);
            yield return new WaitForSeconds(3f);
            popup.HidePopUp();
        }

        ForwardSingles(0);
    }

    void ForwardSingles(int idx)
    {
        if (idx < 6)
        {
            int getPlayer = -1;
            for (int i = 0; i < playerStats.Length; i++)
            {
                if ((playerStats[i].IsAlive()) && (playerStats[i].GetSelected() == idx) && (!playerStats[i].IsDuplicate()))
                {
                    playerStats[i].HighlightSelected();
                    getPlayer = i;
                    break;
                }
            }

            if (getPlayer > -1)
            {
                StartCoroutine(ForwardAnimate(getPlayer, idx +1));
            }
            else
            {
                ForwardSingles(idx + 1);
            }
        }
        else
        {
            StartCoroutine(GetBit());
        }
    }

    IEnumerator ForwardAnimate(int playerIdx, int nextIdx)
    {
        arrow.SetActive(true);
        yield return new WaitForSeconds(1f);
		for (int i = 0; i < playerStats.Length; i++) {
			if (playerStats [i].transform.GetSiblingIndex () < playerStats [playerIdx].transform.GetSiblingIndex ()) {
				playerStats [i].playerModel.Backward ();
			}
		}
		playerStats[playerIdx].Forward();
        yield return new WaitForSeconds(1f);
        arrow.SetActive(false);
        ForwardSingles(nextIdx);
    }

	IEnumerator GetBit(bool lastMeal = false)
    {
        PlayerStats lastPlayer = allPlayer.GetChild(allPlayer.childCount-1).GetComponent<PlayerStats>();
        crunchAnim.gameObject.SetActive(true);
        crunchAnim.SetTrigger("Start");
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].IsAlive())
                playerStats[i].ReadyCards();
        }
		sharkModel.SetEat ();
        yield return new WaitForSeconds(0.5f);
        lastPlayer.GetBit();
        yield return new WaitForSeconds(1.5f);

		if (!lastMeal) {

			if (lastPlayer.IsAlive ()) {
				for (int i = 0; i < playerStats.Length; i++) {
					if (playerStats [i].transform.GetSiblingIndex () < lastPlayer.transform.GetSiblingIndex ()) {
						playerStats [i].playerModel.Backward ();
					}
				}
				lastPlayer.transform.SetAsFirstSibling ();
				lastPlayer.playerModel.Forward ();
			}
			crunchAnim.gameObject.SetActive (false);

			lastPlayer.CheckDeath (graveyard);
			for (int i = 0; i < playerStats.Length; i++) {
				playerStats [i].playerModel.SetPosition (playerStats [i].transform.GetSiblingIndex ());
			}

			if ((allPlayer.childCount <= 2) || (!playerStats [player].IsAlive ()))
				gameOver = true;

			NextRound ();
		} else {
			lastPlayer.transform.SetParent(graveyard);
			lastPlayer.playerModel.gameObject.SetActive (false);
			sharkModel.GoAway ();
		}
    }    

    void GameEnd()
    {
		if ((playerStats[player].transform.GetSiblingIndex() == 0) && (playerStats[player].IsAlive()))
        {
			popup.ShowPopUp(PopUpType.WIN);
        }
        else
        {
			popup.ShowPopUp(PopUpType.LOSE);
        }
		if (allPlayer.childCount == 2) {
			PlayerStats firstPlayer = allPlayer.GetChild (0).GetComponent<PlayerStats> ();
			StartCoroutine(GetBit (true));
			firstPlayer.playerModel.SetWin ();
		}
        popup.actionButton.onClick.AddListener(() => { BackToHome(); });
    }

    void BackToHome()
    {
        fader.FadeOut();
        popup.HidePopUp();
        fader.OnFadeOutFinished += AfterBack;
    }

    void AfterBack()
    {
        fader.OnFadeOutFinished -= AfterBack;
        gameObject.SetActive(false);
        titleScreen.SetActive(true);
    }
}
