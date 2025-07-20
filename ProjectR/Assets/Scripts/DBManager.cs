using Mono.Data.Sqlite;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using UnityEngine;


public static class DBManager
{
    public static SqliteConnection conn;

    public static string src = Path.Combine(Application.streamingAssetsPath, "raceGame.db");
    public static string dst = Path.Combine(Application.persistentDataPath, "raceGame.db");
    public static string dbPath;

    private static readonly object dbLock = new object();

    public static int RankNum;
    public static string UserName;
    public static string bestTime;

    public static void init()
    {
        lock (dbLock)
        {
            if (conn != null) return;

            if (!File.Exists(dst))
            {
                Debug.Log("!");
                File.Copy(src,dst);
            }
            dbPath = dst;

            FileInfo fi = new FileInfo(dbPath);
            if (fi.IsReadOnly) fi.IsReadOnly = false;

            if (File.Exists(dbPath))
            {
                Debug.Log($" DB 파일 존재함: {dbPath}");
            }
            else
            {
                Debug.LogError($" DB 파일 없음: {dbPath}");
            }

            try
            {
                // WAL 활성화는 반드시 Open 이후, 다른 커넥션이 없는 상태에서 실행되어야 함
                string connectionString = $"URI=file:{dbPath};Pooling=False;Cache=Shared;";
                conn = new SqliteConnection(connectionString);
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA journal_mode=WAL;";
                    var result = cmd.ExecuteScalar();
                    Console.WriteLine($"SQLite journal_mode set to: {result}");
                    if (result?.ToString().ToLower() != "wal")
                    {
                        throw new Exception("WAL 설정 실패. 다른 커넥션이 열려 있을 수 있습니다.");
                    }
                }

                // 필요한 경우, 동기화 레벨도 완화
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "PRAGMA synchronous=NORMAL;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB 초기화 중 오류 발생: " + ex.Message);
                throw;
            }

            Debug.Log($"현재 DB 경로: {conn.DataSource}");
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
        lock (dbLock)
        {
            if (PlayerIDCheck(username)) // 같은 락 안에서 실행
            {
                Debug.Log($"{username} 이미 존재함");
                return;
            }

            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO UserDB(username) VALUES (@username)";
                var param = cmd.CreateParameter();
                param.ParameterName = "@username";
                param.Value = username;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.Log($"등록 실패: {ex.Message}");
            }
        }
    }

    private static bool PlayerIDCheck(string username)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM UserDB WHERE username = @username";
        var param = cmd.CreateParameter();
        param.ParameterName = "@username";
        param.Value = username;
        cmd.Parameters.Add(param);
        var result = cmd.ExecuteScalar();
        return Convert.ToInt32(result) > 0;
    }


    public static void SaveTime(string username, string time)
    {
        lock (dbLock)
        {
            using (var checkCmd = conn.CreateCommand())
            {
                checkCmd.CommandText = "SELECT username FROM ScoreDB WHERE username = @username;";
                var checkParam = checkCmd.CreateParameter();
                checkParam.ParameterName = "@username";
                checkParam.Value = username;
                checkCmd.Parameters.Add(checkParam);

                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.Log("점수갱신");
                        reader.Close(); // 명시적 종료 후 업데이트

                        using (var updateCmd = conn.CreateCommand())
                        {
                            updateCmd.CommandText = "UPDATE ScoreDB SET time = @time WHERE username = @username AND time > @time;";
                            var param1 = updateCmd.CreateParameter();
                            param1.ParameterName = "@username";
                            param1.Value = username;

                            var param2 = updateCmd.CreateParameter();
                            param2.ParameterName = "@time";
                            param2.Value = time;

                            updateCmd.Parameters.Add(param1);
                            updateCmd.Parameters.Add(param2);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Debug.Log("새 점수 추가");
                        reader.Close();

                        using (var insertCmd = conn.CreateCommand())
                        {
                            insertCmd.CommandText = "INSERT INTO ScoreDB(username, time) VALUES(@username, @time);";
                            var param1 = insertCmd.CreateParameter();
                            param1.ParameterName = "@username";
                            param1.Value = username;

                            var param2 = insertCmd.CreateParameter();
                            param2.ParameterName = "@time";
                            param2.Value = time;

                            insertCmd.Parameters.Add(param1);
                            insertCmd.Parameters.Add(param2);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }  
    }


    /*public static void InputRank(string username, string time)
    {
        lock (dbLock)
        {
            using (var checkCmd = conn.CreateCommand())
            {
                checkCmd.CommandText = "SELECT username FROM RankDB WHERE username = @username;";
                var checkParam = checkCmd.CreateParameter();
                checkParam.ParameterName = "@username";
                checkParam.Value = username;
                checkCmd.Parameters.Add(checkParam);

                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Debug.Log("점수갱신");
                        reader.Close(); // 명시적 종료 후 업데이트

                        using (var updateCmd = conn.CreateCommand())
                        {
                            updateCmd.CommandText = "UPDATE RankDB SET best_time = @time WHERE username = @username AND best_time > @time;";
                            var param1 = updateCmd.CreateParameter();
                            param1.ParameterName = "@username";
                            param1.Value = username;

                            var param2 = updateCmd.CreateParameter();
                            param2.ParameterName = "@time";
                            param2.Value = time;

                            updateCmd.Parameters.Add(param1);
                            updateCmd.Parameters.Add(param2);

                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Debug.Log("새 점수 추가");
                        reader.Close();

                        using (var insertCmd = conn.CreateCommand())
                        {
                            insertCmd.CommandText = "INSERT INTO RankDB(username, best_time) VALUES(@username, @time);";

                            var param1 = insertCmd.CreateParameter();
                            param1.ParameterName = "@username";
                            param1.Value = username;

                            var param2 = insertCmd.CreateParameter();
                            param2.ParameterName = "@time";
                            param2.Value = time;

                            insertCmd.Parameters.Add(param1);
                            insertCmd.Parameters.Add(param2);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }*/
    public static void InputRank(string username, string newTimeStr)
    {
        lock (dbLock)
        {
            using (var checkCmd = conn.CreateCommand())
            {
                checkCmd.CommandText = "SELECT best_time FROM RankDB WHERE username = @username;";
                var checkParam = checkCmd.CreateParameter();
                checkParam.ParameterName = "@username";
                checkParam.Value = username;
                checkCmd.Parameters.Add(checkParam);

                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string oldTimeStr = reader.GetString(0);
                        reader.Close();
                        if (string.Compare(newTimeStr, oldTimeStr, StringComparison.Ordinal) < 0)
                        {
                            Debug.Log("점수 갱신");

                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.CommandText = "UPDATE RankDB SET best_time = @newTime WHERE username = @username;";

                                var param1 = updateCmd.CreateParameter();
                                param1.ParameterName = "@username";
                                param1.Value = username;

                                var param2 = updateCmd.CreateParameter();
                                param2.ParameterName = "@newTime";
                                param2.Value = newTimeStr;

                                updateCmd.Parameters.Add(param1);
                                updateCmd.Parameters.Add(param2);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            Debug.Log("기존 기록이 더 좋음 → 갱신 안함");
                        }
                    }
                    else
                    {
                        reader.Close();
                        Debug.Log("새 점수 추가");

                        using (var insertCmd = conn.CreateCommand())
                        {
                            insertCmd.CommandText = "INSERT INTO RankDB(username, best_time) VALUES(@username, @time);";

                            var param1 = insertCmd.CreateParameter();
                            param1.ParameterName = "@username";
                            param1.Value = username;

                            var param2 = insertCmd.CreateParameter();
                            param2.ParameterName = "@time";
                            param2.Value = newTimeStr;

                            insertCmd.Parameters.Add(param1);
                            insertCmd.Parameters.Add(param2);

                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }



    public static void JoinAndRanking()
    {
        lock (dbLock)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT row_number() OVER(ORDER BY best_time ASC, username ASC) 'RANK',username,best_time FROM RankDB;";

            using var reader = cmd.ExecuteReader();
        }
    }

    public static void InputCartID(int CartID,string username)
    {
        lock (dbLock)
        {
            using(var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE UserDB SET current_cart_id = @CartID WHERE username = @username;";
                var param1 = cmd.CreateParameter();
                param1.ParameterName = "@CartID";
                param1.Value = CartID;
                var param2 = cmd.CreateParameter();
                param2.ParameterName = "@username";
                param2.Value = username;
                
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);

                cmd.ExecuteNonQuery();

            }
        }
    }
}

/*SELECT R.rank, R.username, R.best_time 
        FROM RankDB R
        INNER JOIN UserDB U ON R.username = U.username
        INNER JOIN ScoreDB S ON R.best_time = S.time
        ORDER BY R.best_time;*/
