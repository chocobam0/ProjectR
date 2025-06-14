using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCart : MonoBehaviour
{
    [SerializeField]
    private UserInfo PlayerName;
    private void Start()
    {
        PlayerName = GameObject.Find("UserInfo").GetComponent<UserInfo>();
    }
    public void SelectBtn(int CartID)
    {
        PlayerName.CartID = CartID;
        DBManager.InputCartID(CartID,PlayerName.userName);
    }
    
}
