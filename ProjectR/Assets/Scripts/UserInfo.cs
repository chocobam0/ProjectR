using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    [SerializeField]
    private LoginManager loginManager;
    public string userName = null;
    public int CartID = 0;
    //public string bestTime = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loginManager = GameObject.Find("LoginPanel").GetComponent<LoginManager>();
    }

    private void Start()
    {
        //if (!File.Exists(DBManager.dst))
        //{
        //    Debug.Log("!");
        //    File.Copy(DBManager.src, DBManager.dst);
        //}
        //DBManager.dbPath = DBManager.dst;
        DBManager.init();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            DBManager.Close();
            Application.Quit();
        }
    }

    public void SavePlayerName(string playerName)
    {
        userName = playerName;
    }
/*    public void SaveBestTIme(string bestTime)
    {
        this.bestTime = bestTime;
    }*/
}
