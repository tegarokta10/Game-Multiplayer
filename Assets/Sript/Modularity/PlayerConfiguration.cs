using UnityEngine.InputSystem; // Menambahkan namespace Input System
using UnityEngine;
using System.Collections.Generic;

public class PlayerConfiguration
{
    public PlayerInput PlayerInput { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}
