using UnityEngine;
using Unity.Netcode;
using System.Text;

public class GameManager : NetworkBehaviour
{
    public GameObject Man;
    public GameObject Girl;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.ConnectionApprovalCallback = ApprovalCheck;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab = Man;
            NetworkManager.Singleton.StartHost();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes("Girl");
            NetworkManager.Singleton.StartClient();
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // The client identifier to be authenticated
        var clientId = request.ClientNetworkId;

        // Additional connection data defined by user code
        var connectionData = request.Payload;

        var _character = Encoding.UTF8.GetString(connectionData);

        if (_character == "Girl")
        {
            response.PlayerPrefabHash = Girl.GetComponent<NetworkObject>().PrefabIdHash;
        }
        else
        {
            response.PlayerPrefabHash = Man.GetComponent<NetworkObject>().PrefabIdHash;
        }

        // Your approval logic determines the following values
        response.Approved = true;
        response.CreatePlayerObject = true;

        // Position to spawn the player object (if null it uses default of Vector3.zero)
        response.Position = Vector3.zero;

        // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
        response.Rotation = Quaternion.identity;

        // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
        // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
        response.Reason = "Some reason for not approving the client";

        // If additional approval steps are needed, set this to true until the additional steps are complete
        // once it transitions from true to false the connection approval response will be processed.
        response.Pending = false;
    }
}

