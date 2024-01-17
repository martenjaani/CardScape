using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownScript : MonoBehaviour
{
    public int HowManySeconds;
    public GameObject CountDownTextPrefab;

    private void Awake()
    {
        for (int i = HowManySeconds; i > 0; i--)
        {
            GameObject text = Instantiate(CountDownTextPrefab, transform);
            text.GetComponent<TextMeshPro>().text = i.ToString();
        }
        GameObject text2 = Instantiate(CountDownTextPrefab, transform);
        text2.GetComponent<TextMeshPro>().text = "GO!";
    }
}
