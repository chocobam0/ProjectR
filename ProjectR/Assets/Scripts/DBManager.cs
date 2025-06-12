using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using UnityEditor.Build.Player;

public static class DBManager
{
    private static IDbConnection conn;

    private static string dbPath = Path.Combine(Application.streamingAssetsPath, "raceGame.db");


    public static void init()
    {
        if(conn == null)
        {
            conn = new SqliteConnection("URI=file: " + dbPath);
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

    public static void PlayerLogin(string username)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT username FROM UserDB WHERE username = @username";
        IDataReader dataReader = cmd.ExecuteReader();

        if(dataReader != null )
        {
            return;
        }
        else
        {
            cmd.CommandText = "INSERT INTO UserDB VALUES (1,@username,1);";
        }
    }
}
