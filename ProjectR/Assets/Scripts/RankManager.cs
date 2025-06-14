using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Rank;
    // Start is called before the first frame update
    void Start()
    {
        Rank = GameObject.Find("Rank1").GetComponentInChildren<TextMeshProUGUI>();
        DBManager.JoinAndRanking();
        ShowRankData();
    }

    private void ShowRankData()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        lock(DBManager.conn)
        {
            using (var cmd = DBManager.conn.CreateCommand())
            {
                cmd.CommandText = "SELECT row_number() OVER(ORDER BY best_time ASC, username ASC) 'RANK',username,best_time FROM RankDB;";

                using(var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int rank = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string bestTime = reader.GetString(2);

                        sb.Append($"{rank} | {username} | {bestTime}\n");
                    }
                }
            }
        }
        Rank.text = sb.ToString();
    }
}
