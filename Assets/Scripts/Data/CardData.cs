using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Game/CardData")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public GameObject icon;
    public AudioClipGroup audioClipGroup;
}
