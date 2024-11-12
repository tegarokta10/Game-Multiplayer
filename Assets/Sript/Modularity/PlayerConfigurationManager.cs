using UnityEngine;
using System.Collections.Generic;

public class PlayerConfigurationManager : MonoBehaviour
{
    public static PlayerConfigurationManager Instance { get; private set; }
    private List<PlayerConfiguration> playerConfigs = new List<PlayerConfiguration>();
    public bool AllPlayersReady => playerConfigs.Count > 1 && playerConfigs.TrueForAll(p => p.IsReady);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddPlayerConfiguration(PlayerConfiguration config)
    {
        playerConfigs.Add(config);
    }

    public PlayerConfiguration GetPlayerConfig(int index)
    {
        return playerConfigs[index];
    }
}

public class PlayerConfiguration
{
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
}
