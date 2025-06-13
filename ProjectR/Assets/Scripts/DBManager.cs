using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using UnityEditor.Build.Player;
using System.ComponentModel;

public static class DBManager
{
    private static IDbConnection conn;

    private static string dbPath = Path.Combine(Application.streamingAssetsPath, "raceGame.db");


    public static void init()
    {
        if(conn == null)
        {
            conn = new SqliteConnection("URI=file:" + dbPath);
            conn.Open();
        }
    }

    public static void Close()
    {
        if(conn != null)
        {
            conn.Close();
            conn = null;
        }
    }

    public static bool PlayerIDCheck(string username)
    {
        var cmd = conn.CreateCommand();
        /*cmd.CommandText = "INSERT INTO UserDB VALUES (1,'@username',1)";*/
        cmd.CommandText = $"SELECT username FROM UserDB WHERE username = @username;";
        /*cmd.CommandText = "SELECT * FROM UserDB";*/

        var param = cmd.CreateParameter();
        param.ParameterName = "@username";
        param.Value = username;
        cmd.Parameters.Add(param);
        /*        cmd.ExecuteNonQuery();*/

        using (var reader = cmd.ExecuteReader())
        {
            
            if (reader.Read())
            {
                Debug.Log($"{username} 이미 있음");
                reader.Dispose();
                return true;
            }
            else
            {
                
                reader.Dispose();
                ////cmd = conn.CreateCommand();
                //cmd.CommandText = $"INSERT INTO UserDB VALUES (2,'{username}',1)";
                //cmd.ExecuteNonQuery();
                return false;
                
            }
        }
        //cmd.ExecuteNonQuery();
        
        //cmd.ExecuteNonQuery();
        //cmd.Dispose();
        /*cmd.CommandText = $"INSERT INTO UserDB VALUES (3,'{username}',1)";*/
        /*IDataReader dataReader = cmd.ExecuteReader();*/

        /*if(dataReader != null )
        {
            return;
        }
        else
        {
            cmd.CommandText = "INSERT INTO UserDB VALUES (1,@username,1);";
        }*/
    }

    public static void PlayerLogin(string username)
    {
        var cmd = conn.CreateCommand();
        if (PlayerIDCheck(username))
        {
            return;
        }
        else
        {
            Debug.Log($"{username} 생성");
            cmd.CommandText = $"INSERT INTO UserDB VALUES ('{username}',NULL);";
        }
        cmd.ExecuteNonQuery();
    }

    public static void SaveTime(string username, string time)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO ScoreDB(username,time) VALUES('{username}','{time}');";
        cmd.ExecuteNonQuery();
    }
}
