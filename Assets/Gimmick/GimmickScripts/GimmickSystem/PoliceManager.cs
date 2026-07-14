using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceManager : GimmickBase
{
    public static PoliceManager Instance { get; private set; }

    /// <summary>
    /// 各ギミックから違反の報告を受け取る
    /// </summary>
    public void ReportViolation(string gimmickName, string violationType)
    {
        if(_activePoliceCar == null && _policePrefab != null)
        {
            if(_activePoliceCar == null && _policePrefab != null && _spawnPoints.Length > 0)
            {
                Transform bestSpawnPoints = GetRandomNearbySpawnPoint();

                if(bestSpawnPoints != null)
                {
                    _activePoliceCar = Instantiate(_policePrefab, bestSpawnPoints.position, bestSpawnPoints.rotation);
                    Debug.Log($"[PoliceManager] 監視ポイント「{bestSpawnPoints}」からパトカー出動");
                }
            }
        }
        
        // TODO: ここで違反の処理を行う（例: スコアの減点、警告メッセージの表示など）
    }

    public override void ResetGimmick()
    {
        if (_activePoliceCar != null)
        {
            Destroy(_activePoliceCar);
            _activePoliceCar = null;
        }
        Debug.Log("ギミックがリセットされました。警察車両を削除しました。");
    }

    [Header("References")]
    [SerializeField] private GameObject _policePrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _spawnPointDistance = 150f;

    private GameObject _activePoliceCar;
    private Transform _playerTransform;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null) _playerTransform = playerGO.transform;
    }

    private Transform GetRandomNearbySpawnPoint()
    {
        if (_playerTransform == null) return _spawnPoints[0];

        List<Transform> nearbyPoints = new List<Transform>();
        Vector3 playerPos = _playerTransform.position;

        foreach(var point in _spawnPoints)
        {
            if(point == null) continue;

            float distance = Vector3.Distance(point.position, playerPos);

            if (distance < _spawnPointDistance)
            {
                nearbyPoints.Add(point);
            }
        }

        if(nearbyPoints.Count == 0)
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        }

        return nearbyPoints[Random.Range(0, nearbyPoints.Count)];
    }
}
