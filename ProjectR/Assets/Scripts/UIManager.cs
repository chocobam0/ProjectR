using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreTimeText;
    [SerializeField]
    private GameObject ScoreTimeObject;
    [SerializeField]
    private TextMeshProUGUI CountDownText;
    [SerializeField]
    private GameObject CountDownObject;
    [SerializeField]
    private TextMeshProUGUI GoalScoreTimeText;
    [SerializeField]
    private GameObject GoalScoreTimeObject;
    [SerializeField]
    private GoalManager goalManager;
    public int STmin;
    public float STsec;
    [SerializeField]
    private float CDsec = 3.0f;
    public bool IsStart = false;
    private void Start()
    {
        goalManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GoalManager>();
        GoalScoreTimeObject.SetActive(false);
    }
    private void Update()
    {
        if (!IsStart)
        {
            CDsec -= Time.deltaTime;
            if (CDsec <= 0)
            {
                IsStart = true;
            }
            CountDownText.text = string.Format("{0:D}", (int)CDsec);
        }
        if (IsStart)
        {
            CountDownObject.SetActive(false);
            STsec += Time.deltaTime;
            if (STsec >= 60)
            {
                STmin++;
                STsec = 0;
            }

            ScoreTimeText.text = string.Format("{0:D2}:{1:F}", STmin, STsec);
        }
        if (goalManager.IsGoal)
        {
            goalManager.GoalSec = STsec;
            goalManager.GoalMin = STmin;
            ScoreTimeObject.SetActive(false);
            GoalScoreTimeObject.SetActive(true);
            GoalScoreTimeText.text = string.Format("{0:D2}:{1:F}", goalManager.GoalMin, goalManager.GoalSec);
        }
    }
}
