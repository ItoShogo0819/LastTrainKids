using UnityEngine;
using System.Collections;

public class CrossingGate : GimmickBase
{
    // リトライ時のリセット（リトライ時はアニメーションせず瞬時に全開に戻す）
    public override void ResetGimmick()
    {
        StopAllCoroutines();
        _isClosed = false;
        if(_gateBarPivot != null)
        {
            _gateBarPivot.localRotation = Quaternion.Euler(0f, 0f, _openRotation);
        }
        Debug.Log("[踏切] リセットされ、ゲートが瞬時に全開になりました。");
    }

    [Header("Referece")]
    [SerializeField] private Transform _gateBarPivot;

    [Header("Settings")]
    [SerializeField] private float _closeRotation = -90f;
    [SerializeField] private float _openRotation = 0f;
    [SerializeField] private float _animationDuration = 4.0f;

    private bool _isClosed = false;

    private void Start()
    {
        StartCoroutine(GateCycleRoutine());
    }

    /// <summary>
    /// ゲート開閉用コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GateCycleRoutine()
    {
        while (true)
        {
            _isClosed = false;
            Debug.Log("[踏切]ゲートを開け始めます");
            yield return StartCoroutine(AnimationGateCoroutine(_openRotation, _animationDuration));
            Debug.Log("[踏切]ゲートが開きました。通行可能です。");
            yield return new WaitForSeconds(10f);

            _isClosed = true;
            Debug.Log("[踏切]ゲートを閉め始めます。");
            yield return StartCoroutine(AnimationGateCoroutine(_closeRotation, _animationDuration));
            Debug.Log("[踏切]ゲートを封鎖！侵入を禁じます。");
            yield return new WaitForSeconds(10f);
        }
    }

    /// <summary>
    /// 指定した角度まで滑らかに回転させるサブコルーチン
    /// </summary>
    private IEnumerator AnimationGateCoroutine(float targetAngle, float duration)
    {
        if (_gateBarPivot == null) yield break;

        Quaternion startRot = _gateBarPivot.localRotation;
        Quaternion endRot = Quaternion.Euler(0f, 0f, targetAngle);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _gateBarPivot.localRotation = Quaternion.Slerp(startRot, endRot, smoothT);

            yield return null;
        }

        _gateBarPivot.localRotation = endRot;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isClosed && other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"))
        {
            // プレイヤーが閉じたゲートに接触した場合、違反を報告する
            PoliceManager.Instance.ReportViolation("CrossingGate", "RailwayCrossing");
        }
    }
}
