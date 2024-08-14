using TMPro;
using UnityEngine;

public class ChatInput : MonoBehaviour {
    [SerializeField] private ChatBox chatBox;

    private TMP_InputField _inputField;

    private void Awake() {
        _inputField = GetComponent<TMP_InputField>();
        if (_inputField) {
            _inputField.onEndEdit.AddListener(SubmitChatMessage);
        }
    }

    private void OnDestroy() {
        if (_inputField) {
            _inputField.onEndEdit.RemoveListener(SubmitChatMessage);
        }
    }

    private void SubmitChatMessage(string text) {
        if (!chatBox) {
            return;
        }
        
        chatBox.SubmitMessage(text);
        _inputField.text = string.Empty;
    }
}