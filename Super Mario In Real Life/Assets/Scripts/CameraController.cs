using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target = null;
    [SerializeField, Range(0f,3f)] private float lookSpeed = 1f;
    [SerializeField, Range(0f, 360f)] private float minRotation = 5f;
    [SerializeField, Range(0f, 360f)] private float maxRotation = 85f;
    [SerializeField, Range(0f, 5f)] private float minDistance = 2f;
    [SerializeField, Range(5f, 40f)] private float maxDistance = 10f;
    private float yaw;
    private float pitch;

    void Start()
    {
    }
    
    void Update()
    {
        MouseMovement();
        float desiredDistance = DesiredDistance();
        transform.position = target.position + new Vector3(Mathf.Sin(yaw) * Mathf.Cos(pitch) * desiredDistance, Mathf.Sin(pitch) * desiredDistance, Mathf.Cos(yaw) * Mathf.Cos(pitch) * desiredDistance);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    private void MouseMovement()
    {
        yaw += (Input.GetAxis("Mouse X") * lookSpeed * 2f * Time.deltaTime) % 360f;
        pitch = Mathf.Clamp(pitch - Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime, minRotation * Mathf.Deg2Rad, maxRotation * Mathf.Deg2Rad);
    }
    private float DesiredDistance()
    {
        float distance = Mathf.Clamp(Vector3.Distance(target.position, transform.position), minDistance, maxDistance);
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel < 0f) distance = Mathf.Clamp(Mathf.Lerp(minDistance, maxDistance, ((distance - minDistance) / (maxDistance - minDistance) + 0.1f)), minDistance, maxDistance);
        if (scrollWheel > 0f) distance = Mathf.Clamp(Mathf.Lerp(minDistance, maxDistance, ((distance - minDistance) / (maxDistance - minDistance) - 0.1f)), minDistance, maxDistance);
        return distance;
    }
}
