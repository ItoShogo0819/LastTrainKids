using UnityEngine;

/// <summary>
/// 全てのGimmickが引き継ぐ共通クラス
/// </summary>
public abstract class GimmickBase : MonoBehaviour
{
    /// <summary>
    /// リトライ時、Gimmickの状態を初期化する
    /// </summary>
    public abstract void ResetGimmick();
}