using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events
{
    public static event Action<bool> DoubleJumpCardActivated;
    public static void DoubleJump() => DoubleJumpCardActivated?.Invoke(true);

    public static event Action<bool> DashCardActivated;
    public static void Dash() => DashCardActivated?.Invoke(true);

    public static event Action OnRestartLevel;
    public static void RestartLevel() => OnRestartLevel?.Invoke();
}
