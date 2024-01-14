using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AvaibleCardsScript : MonoBehaviour
{
    List<GameObject> CardPackObjects = new List<GameObject>();
    List<CardScript> Cards = new List<CardScript>();
    List<KeyCode> KeyCodes = new List<KeyCode>();
    List<int> amountOfCards = new List<int>();
    List<RectTransform[]> images = new List<RectTransform[]>(); 

    public CardScript CardPrefab;
    public GameObject CardPackPrefab;
    private RectTransform AmountPanel;

    public List<CardPack> CardPacks = new List<CardPack>();

    private void Awake()
    {
        Events.cardActivated += CardActivated;
    }

    private void OnDestroy()
    {
        Events.cardActivated -= CardActivated;
    }

    void Start()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(5 * (CardPacks.Count + 1) + 70 * CardPacks.Count, 130);

        for (int i = 0; i < CardPacks.Count; i++)
        {
            CardPackObjects.Add(GameObject.Instantiate(CardPackPrefab, transform));
            AmountPanel = CardPackObjects[i].GetComponentInChildren<RectTransform>();
            images.Add(AmountPanel.GetComponentsInChildren<RectTransform>());

            /*TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = Regex.Split(CardPacks[i].keyCode.ToString(), @"(?<!^)(?=[A-Z])")[Regex.Split(CardPacks[i].keyCode.ToString(), @"(?<!^)(?=[A-Z])").Length-1];
            
            Debug.Log(CardPacks[i].keyCode.ToString());
           
            texts[0].fontSize = 14.5F;
            texts[1].text = CardPacks[i].amountOfCards.ToString() + "x";
            texts[1].fontSize = 14.5F;*/
            if (CardPacks[i].amountOfCards > 5)
                CardPacks[i].amountOfCards = 5;

            SetAmount(CardPacks[i].amountOfCards, i);
            Cards.Add(GameObject.Instantiate(CardPrefab, CardPackObjects[i].transform));
            Cards[i].cardData = CardPacks[i].cardData;
            if (Cards[i].cardData.keyCode.ToString().Equals("LeftShift"))
                Cards[i].cardShortCut.text = "Shift";
            else
                Cards[i].cardShortCut.text = Cards[i].cardData.keyCode.ToString();
            KeyCodes.Add(Cards[i].cardData.keyCode);
            amountOfCards.Add(CardPacks[i].amountOfCards);
        }
    }

    void Update()
    {
        /*foreach (KeyCode keyCode in KeyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                int i = KeyCodes.IndexOf(keyCode);
                if (amountOfCards[i] > 0)
                    Cards[i].Pressed();
            }
        }*/
    }

    public void CardActivated(CardScript script)
    {
        int i = Cards.IndexOf(script);
        amountOfCards[i]--;
        SetAmount(amountOfCards[i], i);
        /*TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = amountOfCards[i].ToString()+"x";*/
        if (amountOfCards[i] == 0) {
            RemoveEachElement(CardPackObjects[i]);
        }
        else {
            Vector2 originalScale = script.GetComponent<RectTransform>().transform.localScale;
            Vector2 squishScale = new Vector2(1f, 0.8f);

            // First, lerp from originalScale to squishScale
            script.GetComponent<RectTransform>().transform.localScale = Vector2.Lerp(originalScale, squishScale, 0.3f);

            // Now, lerp back from squishScale to originalScale
            StartCoroutine(ReturnToOriginalScale());

            IEnumerator ReturnToOriginalScale()
            {
                float timer = 0f;
                float duration = 0.3f; // Adjust the duration as needed

                while (timer < duration)
                {
                    float t = timer / duration;
                    script.GetComponent<RectTransform>().transform.localScale = Vector2.Lerp(squishScale, originalScale, t);

                    timer += Time.deltaTime;
                    yield return null;
                }

                // Ensure it ends at the original scale
                script.GetComponent<RectTransform>().transform.localScale = originalScale;
            }
        }
    }

    private void RemoveEachElement(GameObject cardPackObject)
    {
        RectTransform rectTransform = cardPackObject.GetComponent<RectTransform>();
        foreach(RectTransform transform in rectTransform)
        {
            Destroy(transform.gameObject);
        }
    }

    private void SetAmount(int amount, int index)
    {
        foreach (RectTransform image in images[index])
            image.gameObject.SetActive(false);
        for (int i = 0; i < amount + 2; i++)
            images[index][i].gameObject.SetActive(true);
    }

    public void FindScript()
    {

    }
}

[System.Serializable]
public class CardPack
{
    public CardData cardData;
    public int amountOfCards;
}