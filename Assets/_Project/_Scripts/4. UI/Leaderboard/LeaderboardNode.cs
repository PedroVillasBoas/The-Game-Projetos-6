using TMPro;
using UnityEngine;

public class LeaderboardNode : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI placementText;
    [SerializeField] private TextMeshProUGUI runScoreText;
    [SerializeField] private TextMeshProUGUI runTimeText;
    [SerializeField] private TextMeshProUGUI enemiesDefeatedText;

    public void SetNodeData(string placement, string runScore, string runTime, string EnemiesDefeated)
    {
        placementText.text = placement;
        runScoreText.text = runScore;
        runTimeText.text = runTime;
        enemiesDefeatedText.text = EnemiesDefeated;
    }
}
