using UnityEngine;
using System.Collections;

public class CrossingGate : GimmickBase
{
    public override void ResetGimmick()
    {
        _isClosed = false;
        if(_gateBarPivot != null)
        {
            _gateBarPivot.localRotation = Quaternion.Euler(0f, 0f, _openRotation);
        }
        StopAllCoroutines();
    }

    [Header("Referece")]
    [SerializeField] private Transform _gateBarPivot;

    [Header("Settings")]
    [SerializeField] private float _closeRotation = -90f;
    [SerializeField] private float _openRotation = 0f;

    private bool _isClosed = false;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_isClosed && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            // プレイヤーが閉じたゲートに接触した場合、違反を報告する
            PoliceManager.Instance.ReportViolation("CrossingGate", "RailwayCrossing");
        }
    }
}
