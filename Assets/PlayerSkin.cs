using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerSkin : NetworkBehaviour {
    [SerializeField] private MeshRenderer modelRenderer;
    [SerializeField] private Color enemyColor;

    private void Start() {
        if (IsLocalPlayer) {
            return;
        }

        if (!modelRenderer) {
            return;
        }
        
        modelRenderer.material.color = enemyColor;
    }
}
