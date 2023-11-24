using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoaded : MonoBehaviour
{
    public static Action menuLoaded;
    void Start()
    {
        menuLoaded?.Invoke();
    }
}
