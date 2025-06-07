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
        }
    }
}
