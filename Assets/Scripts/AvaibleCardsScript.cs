using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class AvaibleCardsScript : MonoBehaviour
{
    List<GameObject> CardPackObjects = new List<GameObject>();
    List<CardScript> Cards = new List<CardScript>();
    List<KeyCode> KeyCodes = new List<KeyCode>();
    List<int> amountOfCards = new List<int>();
    List<RectTransform[]> images = new List<RectTransform[]>(); 

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
            CardPackObjects.Add(Instantiate(CardPackPrefab, transform));
            //CardPackObjects.Add(transform.GetChild(i).gameObject);
            Transform panel = CardPackObjects[i].transform.GetChild(0).GetChild(0); //Gets the first child of the CardPack object aka the amount of use panel
            int childCount = panel.childCount; //Gets the use image count which should always be 5
            RectTransform[] tempTransforms = new RectTransform[childCount]; //Empty array for each image
            for(int j = 0; j < childCount; j++) //Adds the images to the array
                tempTransforms[j] = panel.GetChild(j).GetComponentInChildren<RectTransform>();
            images.Add(tempTransforms); //Adds the images array to images list which contains image arrays for all cardpacks

            /*TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = Regex.Split(CardPacks[i].keyCode.ToString(), @"(?<!^)(?=[A-Z])")[Regex.Split(CardPacks[i].keyCode.ToString(), @"(?<!^)(?=[A-Z])").Length-1];
            
            Debug.Log(CardPacks[i].keyCode.ToString());
           
            texts[0].fontSize = 14.5F;
            texts[1].text = CardPacks[i].amountOfCards.ToString() + "x";
            texts[1].fontSize = 14.5F;*/
            if (CardPacks[i].amountOfCards > 5)  //Kui üle viie pantud paneb viie peale et tervet asja ära ei fuck upiks
                CardPacks[i].amountOfCards = 5;

            for (int j = 5 - CardPacks[i].amountOfCards - 1; j >= 0; j--) //disableib üleliigsed amount märgid
                images[i][j].gameObject.GetComponent<Image>().enabled = false;

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

    public void CardActivated(CardScript script)
    {
        int i = Cards.IndexOf(script);
        amountOfCards[i]--;
        RemoveUse(amountOfCards[i], i);

        /*TextMeshProUGUI[] texts = CardPackObjects[i].GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = amountOfCards[i].ToString()+"x";*/

        if (amountOfCards[i] == 0) { //Kaardi eemaldamine
            Vector2 originalScale = script.GetComponent<RectTransform>().transform.localScale;
            Vector2 squishScale = new Vector2(1f, 0f);

            // Now, lerp back from squishScale to originalScale
            StartCoroutine(ToZero());

            IEnumerator ToZero()
            {
                float timer = 0f;
                float duration = 0.3f; // Adjust the duration as needed

                while (timer < duration)
                {
                    float t = timer / duration;
                    script.GetComponent<RectTransform>().transform.localScale = Vector2.Lerp(originalScale, squishScale, t);

                    timer += Time.deltaTime;
                    yield return null;
                }

                // Ensure it ends at the original scale
                script.GetComponent<RectTransform>().transform.localScale = originalScale;
                RemoveEachElement(CardPackObjects[i]);
            }
        }

        else { //Kaardi squishimine ja tagasi kuna kasutusi veel
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

    private void RemoveUse(int amount, int index)
    {
        RectTransform image = images[index][4-amount];
        GameObject imageObject = image.gameObject;
        Image imageImage = imageObject.GetComponent<Image>();
        imageImage.enabled = false;
        ParticleSystem particleSystem = imageObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }
}

[System.Serializable]
public class CardPack
{
    public CardData cardData;
    public int amountOfCards;
}