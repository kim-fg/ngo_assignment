using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class ChatBox : NetworkBehaviour {
    [FormerlySerializedAs("chatLog")] [SerializeField] private TextMeshProUGUI messageBox;

    [Rpc(SendTo.Server)]
    private void SendMessage_ServerRPC(FixedString128Bytes message) {
        // if player is chat banned - dont pass the message to anyone else etc
        ReceiveMessage_EveryoneRPC(message);
    }

    [Rpc(SendTo.Everyone)]
    private void ReceiveMessage_EveryoneRPC(FixedString128Bytes message) {
        messageBox.text += $"\n{message}";
    }
    
    public void SubmitMessage(string message) {
        var chatterId = NetworkManager.Singleton.LocalClientId;
        var userMessage = $"P{chatterId}: {message}";
        var messageBytes = new FixedString128Bytes(userMessage);
        SendMessage_ServerRPC(messageBytes);
    }

    private void Start() {
        messageBox.text = string.Empty;
    }
}