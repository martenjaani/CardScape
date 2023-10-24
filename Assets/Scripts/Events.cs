using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events
{
    public static event Action<bool> DoubleJumpCardActivated;
    public static void DoubleJump() => DoubleJumpCardActivated?.Invoke(true);
}
