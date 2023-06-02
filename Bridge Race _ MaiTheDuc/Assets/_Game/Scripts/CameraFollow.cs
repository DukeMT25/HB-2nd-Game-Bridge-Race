using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 20;

    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
