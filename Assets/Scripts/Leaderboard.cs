using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] List<LeaderboardItem> leaderboardItems = new List<LeaderboardItem>();

    List<LeaderBoardPlayerData> leaderboard = new List<LeaderBoardPlayerData>();

    private void OnEnable()
    {
        refresh();
        for (int i = 0; i < leaderboardItems.Count; i++)
        {
            leaderboardItems[i].SetUp(leaderboard[i].playerName, leaderboard[i].Score.ToString());
        }
    }

    void refresh()
    {
        // Get leaderboard
        MetafabManager.GetLeaderboard(5, (res) => {
            leaderboard = res;
        });
    }

}
