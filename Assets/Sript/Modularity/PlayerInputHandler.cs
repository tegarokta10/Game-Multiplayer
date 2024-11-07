using UnityEngine;
using Unity.Netcode;

public class PlayerInputHandler : NetworkBehaviour
{
    private PlayerConfiguration playerConfig;
    private Mover mover;

    public void InitializePlayer(PlayerConfiguration config)
    {
        playerConfig = config;
        mover = GetComponent<Mover>();

        GetComponent<Renderer>().material = playerConfig.PlayerMaterial;
    }

    private void Update()
    {
        if (IsOwner && playerConfig != null && playerConfig.IsReady)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float moveDirection = Input.GetAxis("Vertical");
        mover.Move(moveDirection);
    }
}
