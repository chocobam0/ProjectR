using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI ScoreTImeText;
    private int min;
    private float sec;
    private void Update()
    {
        sec += Time.deltaTime;
        if(sec >= 60)
        {
            min++;
            sec = 0;
        }

        ScoreTImeText.text = string.Format("{0:D2}:{1:F}", min, sec);
    }
}
