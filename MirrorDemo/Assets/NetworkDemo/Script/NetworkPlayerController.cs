using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Mirror;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerController : NetworkBehaviour
{
    public ThirdPersonController ThirdPersonController;
    public PlayerInput PlayerInput;
    public GameObject FollowCameraPrefab;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        ThirdPersonController.enabled = true;
        PlayerInput.enabled = true;

        GameObject go = GameObject.Instantiate(FollowCameraPrefab);
        go.GetComponent<CinemachineVirtualCamera>().Follow = transform.Find("PlayerCameraRoot");
    }
}
