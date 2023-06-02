using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetting : MonoBehaviour
{
    
    private Rigidbody _rigidbody;
    private Transform _playerPosition;
    
    private HealthController _healthController;
    
    [SerializeField]
    private float attackCooldown = 0.7f;
    
    private float _lastAttackTime = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerPosition = GameObject.Find("Player").GetComponentInChildren<CapsuleCollider>().transform;
        _healthController = GameObject.Find("HealthController").GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = _playerPosition.position;
        var enemyPosition = transform.position;
        
        var newRotations = Quaternion.LookRotation(playerPosition - enemyPosition);
        newRotations.x = 0;
        newRotations.z = 0;
        _rigidbody.MoveRotation(newRotations);
        _rigidbody.AddRelativeForce(Vector3.forward * 5f, ForceMode.Force);

        var distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);
        
        _lastAttackTime += Time.deltaTime;
        if (distanceToPlayer > 10f) return;
        if (_lastAttackTime < attackCooldown) return;
        _healthController.TakeDamage(Random.Range(2, 6));
        _lastAttackTime = 0f;
    }
}
