using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public bool IsGoal = false;
    [SerializeField]
    private bool IsHalf = false;
    [SerializeField]
    private int labNum = 0;
    /*[SerializeField]
    private GameObject StartPoint;
    [SerializeField]
    private GameObject HalfPoint;*/
    public float GoalSec;
    public int GoalMin;
    [SerializeField]
    private UserInfo PlayerName;


    private void Start()
    {
        Time.timeScale = 1.0f;
        PlayerName = GameObject.Find("UserInfo").GetComponent<UserInfo>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Half")
        {
            IsHalf = true;
        }
        if (IsHalf)
        {
            if(other.tag == "Start")
            {
                labNum++;
                IsHalf = false;
                if(labNum == 2)
                {
                    IsGoal = true;
                }
            }
        }
    }

    private void Update()
    {
        if(IsGoal == true)
        {
            Time.timeScale = 0;
            //시간 저장
            //if(GoalMin >= 0)
            //{
            //    GoalMin--;
            //    GoalSec += 60.0f;
            //}
            string time1 = string.Format("{0:00}:{1:F}", GoalMin, GoalSec);
            DBManager.SaveTime(PlayerName.userName, time1);
            DBManager.InputRank(PlayerName.userName, time1);
            IsGoal = false;
        }
    }
}
