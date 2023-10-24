using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaibleCardsScript : MonoBehaviour
{
    List<RectTransform> Cards = new List<RectTransform>();
    void Start()
    {
        foreach (RectTransform card in transform) //Lisab kõik hetkel selle küljes olevad kaardid listi
        {
            Cards.Add(card);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //Kui vajutab Q siis võtab listist esimese kaardi ning aktiveerib selles oleva Pressed funktsiooni.
        {
            if(Cards.Count > 0)
            {
                CardScript card = Cards[0].GetComponent<CardScript>();
                if (card != null)
                {
                    card.Pressed();
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
        Cards.Remove(cardRect);
    }
}
