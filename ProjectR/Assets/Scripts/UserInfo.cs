using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    [SerializeField]
    private LoginManager loginManager;
    public string userName = null;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        loginManager = GameObject.Find("LoginPanel").GetComponent<LoginManager>();
    }
    public void SavePlayerName(string playerName)
    {
        userName = playerName;
    }
}
