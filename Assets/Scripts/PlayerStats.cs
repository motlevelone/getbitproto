using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public PlayerColor playerColor;
	public Image playerIcon;
	public Sprite[] icons;
    public Image[] cards;
    Color used = Color.gray;
    Color ready = Color.white;
    public Image cardSelected;
    public Animator cardSelectedAnim;
    List<int> availableCards;
    public Sprite[] cardImages;
    int selected;
    int life;
    public Text lifeLabel;
    public GameObject youLabel;
	public PlayerModel playerModel;

    public void InitPlayer(bool realPlayer,int playerVariant)
    {
        gameObject.SetActive(true);
		playerColor = (PlayerColor)playerVariant;
		playerIcon.sprite = icons [playerVariant];
		playerModel.playerColor = playerColor;
		playerModel.gameObject.SetActive (false);
		playerModel.gameObject.SetActive (true);
        life = 1;
        lifeLabel.text = "" + life;
        selected = -1;
        InitCards();
        youLabel.SetActive(realPlayer);
    }

    public void InitCards(bool fromReady = false)
    {
        availableCards = new List<int>();
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].color = ready;
            int selIdx = i;
            availableCards.Add(selIdx);
        }
        if (!fromReady)
        {
            cardSelectedAnim.SetInteger("State", 0);
            cardSelected.gameObject.SetActive(false);
        }
    }

    public bool IsPlayer()
    {
        return youLabel.activeSelf;
    }
    public bool IsCardReady(int idx)
    {
        return (cards[idx].color.Equals(ready));
    }

    public void SelectCards(int sel = -1)
    {
        if (sel < 0)
        {
            selected = availableCards[Random.Range(0, availableCards.Count)];
            availableCards.Remove(selected);
        }
        else
        {
            selected = sel;
        }
        cardSelected.sprite = cardImages[selected];
        cardSelected.gameObject.SetActive(true);
		cardSelectedAnim.SetInteger("State", 0);
    }

    public int GetSelected()
    {
        return selected;
    }

    public void DuplicateSelected()
    {
        cardSelectedAnim.SetInteger("State", 1);
    }

    public bool IsDuplicate()
    {
        return (cardSelectedAnim.GetInteger("State")==1);
    }

    public void HighlightSelected()
    {
        cardSelectedAnim.SetInteger("State", 2);
    }

    public void Forward()
    {
        transform.SetAsFirstSibling();
        cardSelectedAnim.SetInteger("State", 3);
		playerModel.Forward ();
    }

    public void ReadyCards()
    {
        cards[selected].color = used;
        availableCards.Remove(selected);
        cardSelectedAnim.SetInteger("State", 4);
        if (availableCards.Count <= 2)
            InitCards(true);
    }

    public void GetBit()
    {
		playerModel.GetBit ();
        life--;
        lifeLabel.text = "" + life;
        InitCards(true);
    }

    public void CheckDeath(Transform graveyard)
    {
        if (life <= 0)
        {
            transform.SetParent(graveyard);
			playerModel.gameObject.SetActive (false);
        }
    }

    public bool IsAlive()
    {
        return (life > 0);
    }


}

