using UnityEngine;

public class GoalArea : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager _gameManager;

    private bool _isPlayerInGoalArea = false;
    private Rigidbody _playerRB;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInGoalArea = true;
            _playerRB = other.GetComponentInParent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
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
