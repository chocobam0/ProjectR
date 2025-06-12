using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoginBtn(string username)
    {
        DBManager.PlayerLogin(username);
    }
}
