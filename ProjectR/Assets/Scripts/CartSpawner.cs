using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] CartPrefabs;
    [SerializeField]
    private Transform StartPosition;
    [SerializeField]
    private int CartIndex;
    private UserInfo CartID;


    private void Awake()
    {
        CartID = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        CartIndex = CartID.CartID - 1;
        Instantiate(CartPrefabs[CartIndex], StartPosition);
    }
}
