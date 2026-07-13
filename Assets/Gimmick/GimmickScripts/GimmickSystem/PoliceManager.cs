using UnityEngine;

public class PoliceManager : MonoBehaviour
{
    public static PoliceManager Instance { get; private set; }

    /// <summary>
    /// 各ギミックから違反の報告を受け取る
    /// </summary>
    public void ReportViolation(string gimmickName, string violationType)
    {
        Debug.Log($"違反報告: ギミック名: {gimmickName}, 違反タイプ: {violationType}");
        // TODO: ここで違反の処理を行う（例: スコアの減点、警告メッセージの表示など）
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
}
