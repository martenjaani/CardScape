using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelData")]
public class LevelData : ScriptableObject
{
    public string LevelName;
    public string SceneName;
    public string BestTime;
    public Sprite IconSprite;
}
