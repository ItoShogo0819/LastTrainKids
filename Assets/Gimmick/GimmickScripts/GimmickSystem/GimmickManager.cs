using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// scene全体のギミックのライフサイクルを統括するマネージャー(主にリトライ時のリセット)
/// </summary>
public class GimmickManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameManager _gameManager;

    private readonly List<GimmickBase> _gimmicks = new List<GimmickBase>();

    private void Start()
    {
        var foundGimmicks = FindObjectsByType<GimmickBase>(FindObjectsInactive.Exclude);
        _gimmicks.AddRange(foundGimmicks);

        if(_gameManager != null)
        {
            _gameManager.RetryRequested += OnRetryRequested;
        }
    }

    private void OnDestry()
    {
        if(_gameManager != null)
        {
            _gameManager.RetryRequested -= OnRetryRequested;
        }
    }

    /// <summary>
    /// リトライ要求を受け取ったとき、全ギミックを一斉リセット
    /// </summary>
    private void OnRetryRequested()
    {
        foreach (var gimmick in _gimmicks)
        {
            if(gimmick != null)
            {
                gimmick.ResetGimmick();
            }
        }

        Debug.Log($"[GimmickManager] {_gimmicks.Count}個のギミックをリセットしました");
    }
}
