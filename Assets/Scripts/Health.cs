using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour {
    [SerializeField] private int maxAmount = 3;

    private NetworkVariable<int> _currentAmount = new();
    
    public bool IsDead { get; private set; }

    private void Start() {
        if (IsServer) {
            _currentAmount.Value = maxAmount;
        }
    }

    private void Die() {
        if (!IsServer) {
            return;
        }
        
        IsDead = true;
        Destroy(gameObject);
    }

    public void DoDamage(ulong sourceClientId, int amount = 1) {
        if (!IsServer) {
            return;
        }
        
        if (IsDead) {
            return;
        }
        
        _currentAmount.Value -= amount;

        if (_currentAmount.Value <= 0) {
            Die();
        }
    }
}
