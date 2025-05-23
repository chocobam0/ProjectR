using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float RotateSpeed = 100;
    [SerializeField]
    private float acceler = 0.0f;
    [SerializeField]
    private float accelerSpeed = 1.6f;
    [SerializeField]
    private float currentInput = 0.0f;

    private void Update()
    {
        float zz = Input.GetAxis("Vertical");
        float xx = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.UpArrow))
        {
            /*StartCoroutine("CountTimer");*/
            currentInput += Time.deltaTime;

            if(currentInput >= 3)
            {
                if(acceler < accelerSpeed)
                {
                    acceler += accelerSpeed * Time.deltaTime * 0.3f;
                }
                transform.Translate(Vector3.right * speed*acceler * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            currentInput = 0.0f;
            accelerSpeed = 0.0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(0, xx * RotateSpeed * Time.deltaTime, 0));
        }
    }

    /*private IEnumerable CountTimer()
    {
        Debug.Log("코루틴실행");
        if (currentInput < 3)
        {
            yield return new WaitForSeconds(1.0f);
            currentInput++;
            StartCoroutine("CountTimer");
        }
    }*/
}
