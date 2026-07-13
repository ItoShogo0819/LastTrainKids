using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム状態の遷移を管理し、状態変更イベントを各システムに通知
/// </summary>
public class GameManager : MonoBehaviour
{
    public event Action<GameState> StateChanged;
    public event Action RetryRequested;

    public GameState CurrentState { get; private set; } = GameState.MainMenu;

    public void OnStartGame()
    {
        ChangeState(GameState.Playing);
    }

    public void OnGameOver()
    {
        if (CurrentState == GameState.GameOver) return;

        ChangeState(GameState.GameOver);
    }

    public void OnGameClear()
    {
        if (CurrentState == GameState.Clear) return;
        Debug.Log("Game Clear");

        ChangeState(GameState.Clear);
    }

    public void OnRetry()
    {
        RetryRequested?.Invoke();

        ChangeState(GameState.Playing);
    }

    public void PauseToggle()
    {
        if(CurrentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
        }
        else if(CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            ChangeState(GameState.MainMenu);
        }
        else if (scene.name == "InGame")
        {
            ChangeState(GameState.Playing);
        }
    }

    private void ChangeState(GameState nextState)
    {
        if (CurrentState == nextState) return;

        CurrentState = nextState;
        StateChanged?.Invoke(CurrentState);
    }
}
