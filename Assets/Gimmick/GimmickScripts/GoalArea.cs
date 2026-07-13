using System.Transactions;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoalArea : GimmickBase
{
    public override void ResetGimmick()
    {
        _isPlayerInGoalArea = false;
        _playerRB = null;
    }

    [Header("References")]
    [SerializeField] private GameManager _gameManager;

    private bool _isPlayerInGoalArea = false;
    private Rigidbody _playerRB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            _isPlayerInGoalArea = true;
            _playerRB = other.attachedRigidbody;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            _isPlayerInGoalArea = false;
            _playerRB = null;
        }
    }

    private void Update()
    {
        if(_isPlayerInGoalArea && _gameManager != null && _gameManager.CurrentState == GameState.Playing)
        {
            if(_playerRB != null)
            {
                if(_playerRB.linearVelocity.magnitude < 0.1f)
                {
                    _gameManager.OnGameClear();
                    _isPlayerInGoalArea = false;
                }
            }
        }
    }
}
