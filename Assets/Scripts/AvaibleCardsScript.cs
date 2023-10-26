using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AvaibleCardsScript : MonoBehaviour
{
    List<RectTransform> CardPacks = new List<RectTransform>();
    List<CardScript> Cards = new List<CardScript>();

    public CardScript CardPrefab;

    private int amountOfFirstCards = 3;
    private int amountOfSecondCards = 2;

    public CardData FirstCardData;
    public CardData SecondCardData;

    void Start()
    {
        foreach (RectTransform cardPacks in transform) //Lisab kõik hetkel selle küljes olevad kaardid listi
        {
            CardPacks.Add(cardPacks);
        }

        if (CardPacks.Count == 2)
        {
            RectTransform cardPack = CardPacks[0];
            TextMeshProUGUI[] texts = cardPack.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "Q";
            texts[1].text = amountOfFirstCards.ToString();
            CardScript card = Instantiate(CardPrefab, CardPacks[0]);
            Cards.Add(card);
            card.cardData = FirstCardData;

            RectTransform cardPack2 = CardPacks[1];
            TextMeshProUGUI[] texts2 = cardPack2.GetComponentsInChildren<TextMeshProUGUI>();
            texts2[0].text = "E";
            texts2[1].text = amountOfSecondCards.ToString();
            CardScript card2 = Instantiate(CardPrefab, CardPacks[1]);
            Cards.Add(card2);
            card2.cardData = SecondCardData;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //Kui vajutab Q siis võtab listist esimese kaardi ning aktiveerib selles oleva Pressed funktsiooni.
        {
            if(amountOfFirstCards > 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    if (!playerMovement.onGround() && !playerMovement.ActivatedDoubleJump)
                    {
                        Cards[0].Pressed();
                        amountOfFirstCards--;
                        TextMeshProUGUI[] texts = CardPacks[0].GetComponentsInChildren<TextMeshProUGUI>();
                        texts[1].text = amountOfFirstCards.ToString();
                        if (amountOfFirstCards == 0)
                        {
                            foreach (RectTransform elements in CardPacks[0]) //Lisab kõik hetkel selle küljes olevad kaardid listi
                            {
                                Destroy(elements.gameObject);
                            }
                        }
                    }
                    else
                        return;
                }
                else
                    return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) //Kui vajutab Q siis võtab listist esimese kaardi ning aktiveerib selles oleva Pressed funktsiooni.
        {
            if (amountOfSecondCards > 0)
            {
                Cards[1].Pressed();
                amountOfSecondCards--;
                TextMeshProUGUI[] texts = CardPacks[1].GetComponentsInChildren<TextMeshProUGUI>();
                texts[1].text = amountOfSecondCards.ToString();
                if (amountOfSecondCards == 0)
                {
                    foreach (RectTransform elements in CardPacks[1]) //Lisab kõik hetkel selle küljes olevad kaardid listi
                    {
                        Destroy(elements.gameObject);
                    }
                }
            }
        }
    }

    public void AddNewCard() //Hiljem kui kaarte juurde lisada
    {

    }

    public void RemoveCard(GameObject card)
    {
        RectTransform cardRect = card.GetComponent<RectTransform>();
        CardPacks.Remove(cardRect);
    }
}
