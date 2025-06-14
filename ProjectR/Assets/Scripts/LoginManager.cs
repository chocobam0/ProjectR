using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField playerNameInput;
    public string playerName = null;
    [SerializeField]
    private UserInfo userInfo;
    private void Awake()
    {
        playerName = playerNameInput.GetComponent<TMP_InputField>().text;
        userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
    }

    private void Update()
    {
        if(playerName.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            playerName = playerNameInput.text;
            SceneManager.LoadScene("SelectScene");
        }
    }
    //private void Start()
    //{
    //    DBManager.init();
    //}
    // Start is called before the first frame update
    public void LoginBtn()
    {
        playerName = playerNameInput.text;
        DBManager.PlayerLogin(playerName);
        userInfo.SavePlayerName(playerName);
        SceneManager.LoadScene("SelectScene");
    }
}
