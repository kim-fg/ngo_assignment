using Unity.Netcode;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class NetworkManagerUI : MonoBehaviour {
    private NetworkManager _networkManager;

    private void Awake() {
        _networkManager = GetComponent<NetworkManager>();
        if (!_networkManager) {
            enabled = false;
            return;
        }
    }

    private void OnGUI() {
        if (GUILayout.Button("Host")) {
            _networkManager.StartHost();
            return;
        }

        if (GUILayout.Button("Join")) {
            _networkManager.StartClient();
            return;
        }

        if (_networkManager.IsListening) {
            if (GUILayout.Button("Disconnect")) {
                _networkManager.Shutdown();
            }
        }
        else {
            if (GUILayout.Button("Quit")) {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }
}
