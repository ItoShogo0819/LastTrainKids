using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PoliceController : MonoBehaviour
{
    private Transform _playerTarget;
    private NavMeshAgent _agent;
    private GameManager _gameManager;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null)
        {
            _playerTarget = playerGO.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene.");
        }
    }

    private void Update()
    {
        if(_agent != null && _playerTarget != null)
        {
            _agent.SetDestination(_playerTarget.position);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.attachedRigidbody != null && col.collider.attachedRigidbody.CompareTag("Player"))
        {
            Debug.Log("[Police] 確保！ゲームオーバーにします。");
            if(_gameManager != null)
            {
                _gameManager.OnGameOver();
            }
        }
    }
}
