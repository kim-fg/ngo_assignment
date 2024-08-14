using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody _body;

    #region GameLoop

    private void Awake() {
        _body = GetComponent<Rigidbody>();
    }

    private void Start() {
        if (IsServer) {
            _body.velocity = transform.forward * speed;
            Invoke(nameof(KillSelf), lifeTime);
        }
    }
    
    #endregion

    #region Server

    private void OnTriggerEnter(Collider other) {
        if (!IsServer) {
            return;
        }

        if (!other.transform.root.TryGetComponent(out Health health)) {
            return;
        }
        
        health.DoDamage(OwnerClientId);
        KillSelf();
    }
    
    private void KillSelf() {
        Destroy(gameObject);
    }

    #endregion
}