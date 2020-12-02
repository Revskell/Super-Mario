using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target = null;
    [SerializeField, Range(0f,3f)] private float lookSpeed = 1f;
    [SerializeField, Range(0f, 360f)] private float minRotation = 0f;
    [SerializeField, Range(0f, 360f)] private float maxRotation = 80;
    private float distance;
    private float yaw;
    private float pitch;

    void Start()
    {
    }
    
    void Update()
    {
        MouseMovement();


        transform.position = target.position + new Vector3(Mathf.Sin(yaw) * Mathf.Cos(pitch) * distance, Mathf.Sin(pitch) * distance, Mathf.Cos(yaw) * Mathf.Cos(pitch) * distance);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    private void MouseMovement()
    {
        yaw += (Input.GetAxis("Mouse X") * lookSpeed * 2f * Time.deltaTime) % 360f;
        pitch = Mathf.Clamp(pitch + Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime, minRotation * Mathf.Rad2Deg, maxRotation * Mathf.Deg2Rad);
        distance = Vector3.Distance(target.position, transform.position);
    }
}
