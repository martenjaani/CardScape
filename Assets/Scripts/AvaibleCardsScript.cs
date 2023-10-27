using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AvaibleCardsScript : MonoBehaviour
{
    List<GameObject> CardPackObjects = new List<GameObject>();
    List<CardScript> Cards = new List<CardScript>();
    List<KeyCode> KeyCodes = new List<KeyCode>();
    List<int> amountOfCards = new List<int>();

    public CardScript CardPrefab;
    public GameObject CardPackPrefab;

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
            TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = CardPacks[i].keyCode.ToString();
            texts[1].text = CardPacks[i].amountOfCards.ToString();
            Cards.Add(GameObject.Instantiate(CardPrefab, CardPackObjects[i].transform));
            Cards[i].cardData = CardPacks[i].cardData;
            KeyCodes.Add(CardPacks[i].keyCode);
            amountOfCards.Add(CardPacks[i].amountOfCards);
        }
    }

    void Update()
    {
        foreach (KeyCode keyCode in KeyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                int i = KeyCodes.IndexOf(keyCode);
                if (amountOfCards[i] > 0)
                    Cards[i].Pressed();
            }
        }
    }

    public void CardActivated(CardScript script)
    {
        int i = Cards.IndexOf(script);
        amountOfCards[i]--;
        TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = amountOfCards[i].ToString();
        if (amountOfCards[i] == 0)
            RemoveEachElement(CardPackObjects[i]);
    }

    private void RemoveEachElement(GameObject cardPackObject)
    {
        RectTransform rectTransform = cardPackObject.GetComponent<RectTransform>();
        foreach(RectTransform transform in rectTransform)
        {
            Destroy(transform.gameObject);
        }
    }
}

[System.Serializable]
public class CardPack
{
    public CardData cardData;
    public int amountOfCards;
    public KeyCode keyCode;
}