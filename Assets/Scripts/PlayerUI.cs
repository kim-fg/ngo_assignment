using Unity.Netcode;

public class PlayerUI : NetworkBehaviour
{
    private void Start() {
        if (!IsLocalPlayer) {
            gameObject.SetActive(false);
            return;
        }
    }
}
