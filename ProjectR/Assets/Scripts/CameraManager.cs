using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Target;

    public float OffsetX = 0.0f;
    public float OffsetY = 10.0f;
    public float OffsetZ = -10.0f;

    public float angleX = 0.0f;
    public float angleY = 0.0f;
    public float angleZ = 0.0f;

    [SerializeField]
    private Camera PlayerCamera;

    private void FixedUpdate()
    {
        
    }
    private void Start()
    {
        Target = GameObject.FindWithTag("Player");
        PlayerCamera.transform.parent = Target.transform;
        PlayerCamera.transform.localPosition = Vector3.zero;
        PlayerCamera.transform.localPosition = new Vector3(-10.0f, 6.0f, 0);
        PlayerCamera.transform.localRotation = Quaternion.Euler(angleX, angleY, angleZ);
    }
}
