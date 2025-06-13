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

    private void Awake()
    {
        Instantiate(CartPrefabs[CartIndex],StartPosition);
    }
}
