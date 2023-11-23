using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public CardData cardData;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardShortCut;

    private AvaibleCardsScript cardsScript;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        cardsScript = transform.parent.GetComponent<AvaibleCardsScript>(); //Kaartide paneeli scripti, et pärast selle kaardi hävimist
                                                                           //see kaart listist eemaldada
    }

    private void Start()
    {
        cardName.text = cardData.cardName;
        if(cardData.icon != null)
            GameObject.Instantiate(cardData.icon, cardName.GetComponent<RectTransform>().position, Quaternion.identity, transform);
    }

    public void Pressed()
    {
        if (!Events.GetMovementDisabled())       // Checkime kas movement on lubatud
        {
            if (cardName.text.Equals("Double Jump"))
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                    if (!Events.GetPlayerOnGround() && !playerMovement.ActivatedDoubleJump)
                        Events.DoubleJump();
                    else
                        return;
                }
                else
                    return;
            }

            if (cardName.text.Equals("Dash"))
                Events.Dash();

            if (cardName.text.Equals("Ultra Dash"))
                Events.UltraDash();

            Events.CardActivated(this);
        }
    }
}
