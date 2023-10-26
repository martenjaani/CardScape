using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public CardData cardData;
    public TextMeshProUGUI cardName;

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
    }

    public void Pressed()
    {
        if (cardName.text.Equals("Double Jump"))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (!playerMovement.onGround() && !playerMovement.ActivatedDoubleJump)
                    Events.DoubleJump();
                else
                    return;
            }
            else
                return;
        }

        if (cardName.text.Equals("Dash")) Events.Dash();
        
        //cardsScript.RemoveCard(gameObject);
        //Destroy(gameObject);
    }
}
