using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float accelerSpeed = 1.0f;
    [SerializeField]
    private float currentInput = 0.0f;
    private Rigidbody rigid;
    [SerializeField]
    private UIManager UIManager;

    private void Start()
    {
        UIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        rigid = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        /*var dir = Vector3.left;

        rigid.MovePosition(rigid.position + transform.TransformDirection(-dir) * (speed * Time.deltaTime));*/
        if (UIManager.IsStart)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                currentInput += Time.deltaTime;

                if (currentInput >= 3)
                {
                    if (acceler < accelerSpeed)
                    {
                        acceler += accelerSpeed * Time.deltaTime * 0.3f;

                    }
                    rigid.AddForce(transform.TransformDirection(Vector3.right) * speed * acceler);
                }
                else
                {
                    rigid.AddForce(transform.TransformDirection(Vector3.right) * speed);
                }

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                rigid.AddForce(transform.TransformDirection(Vector3.left) * speed * 0.3f);
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                currentInput = 0.0f;
                acceler = 1.0f;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                /*rigid.rotation = rigid.rotation * Quaternion.Euler(0.0f, -RotateSpeed * Time.deltaTime, 0.0f);*/
                transform.Rotate(new Vector3(0, h * RotateSpeed * Time.deltaTime, 0));
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(new Vector3(0, h * RotateSpeed * Time.deltaTime, 0));
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag != "Ground")
        {
            currentInput = 0.0f;
            acceler = 1.0f;
        }
    }
}
