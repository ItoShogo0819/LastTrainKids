using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public event Action<float> TimeChanged;
    public event Action WarningTimeReached;
    public event Action TimeUp;
    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [Header("Settings")]
    [SerializeField] private float _timeLimit = 180f;
    private float _currentTime;
    private bool _isTimerRunning;
    private bool _warningTriggered;
    private void Start()
    {
        ResetTimer();
        if (_gameManager != null)
        {
            _gameManager.StateChanged += OnGameStateChanged;
            _gameManager.RetryRequested += OnRetryRequested;
        }
    }
    
    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.StateChanged -= OnGameStateChanged;
            _gameManager.RetryRequested -= OnRetryRequested;
        }
    }

    // 2. カウントダウンとイベント発火処理を実装
    private void Update()
    {
        if (!_isTimerRunning) return;
        // 残り時間を減算
        _currentTime -= Time.deltaTime;
        // UI等の更新用に毎フレーム残り時間を通知
        TimeChanged?.Invoke(_currentTime);
        // 残り30秒以下になった瞬間に警告イベントを1度だけ呼ぶ
        if (_currentTime <= 30f && !_warningTriggered)
        {
            _warningTriggered = true;
            WarningTimeReached?.Invoke();
        }
        // タイムアップ判定
        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            _isTimerRunning = false;
            TimeUp?.Invoke();

            // GameManagerにゲームオーバーを伝える
            if (_gameManager != null)
            {
                _gameManager.OnGameOver();
            }
        }
    }
    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                StartTimer();
                break;
            case GameState.Paused:
                PauseTimer();
                break;
            case GameState.GameOver:
            case GameState.Clear:
                StopTimer();
                break;
        }
    }
    private void OnRetryRequested()
    {
        ResetTimer();
    }
    // 3. 不足していたメソッドを追加
    private void StartTimer()
    {
        _isTimerRunning = true;
    }
    private void PauseTimer()
    {
        _isTimerRunning = false;
    }
    private void StopTimer()
    {
        _isTimerRunning = false;
    }
    private void ResetTimer()
    {
        _currentTime = _timeLimit;
        _warningTriggered = false;
        _isTimerRunning = false;
        TimeChanged?.Invoke(_currentTime);
    }
}
