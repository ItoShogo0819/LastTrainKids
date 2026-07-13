using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TimeManager _timeManager;

    [Header("UI Panels")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private GameObject _pausePanel;

    [Header("UI Text Components")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _resultText;

    [Header("Timer Settings")]
    [SerializeField] private Color _normalTimerColor = Color.white;
    [SerializeField] private Color _warningTimerColor = Color.red;
    [SerializeField] private float _warningTimeThreshold = 30f;

    private void Start()
    {
        // パネルの初期化
        if (_resultPanel != null) _resultPanel.SetActive(false);
        if (_pausePanel != null) _pausePanel.SetActive(false);

        // イベント購読
        if (_timeManager != null)
        {
            _timeManager.TimeChanged += OnTimeChanged;
        }

        if (_gameManager != null)
        {
            _gameManager.StateChanged += OnGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        // イベント解除
        if (_timeManager != null)
        {
            _timeManager.TimeChanged -= OnTimeChanged;
        }

        if (_gameManager != null)
        {
            _gameManager.StateChanged -= OnGameStateChanged;
        }
    }

    private void OnTimeChanged(float remainingTime)
    {
        if (_timerText == null) return;

        // 残り時間の表示フォーマット (分:秒.ミリ秒 などもお好みで可能。ここでは分:秒)
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // 残り時間に応じた警告カラー変更
        if (remainingTime <= _warningTimeThreshold)
        {
            _timerText.color = _warningTimerColor;
            
            // シンプルな点滅演出 (残り30秒以下の時のみ)
            float pingPong = Mathf.PingPong(Time.time * 2f, 1f);
            _timerText.alpha = Mathf.Lerp(0.3f, 1f, pingPong);
        }
        else
        {
            _timerText.color = _normalTimerColor;
            _timerText.alpha = 1f;
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                if (_resultPanel != null) _resultPanel.SetActive(false);
                if (_pausePanel != null) _pausePanel.SetActive(false);
                break;

            case GameState.Paused:
                if (_pausePanel != null) _pausePanel.SetActive(true);
                break;

            case GameState.Clear:
                ShowResult("GAME CLEAR!", Color.green);
                break;

            case GameState.GameOver:
                ShowResult("GAME OVER...", Color.red);
                break;
        }
    }

    private void ShowResult(string message, Color color)
    {
        if (_resultPanel != null) _resultPanel.SetActive(true);
        if (_resultText != null)
        {
            _resultText.text = message;
            _resultText.color = color;
        }
    }
}
