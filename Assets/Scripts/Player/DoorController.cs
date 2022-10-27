using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float speed = 60f; // in degrees per second
    public float maxAngle = 90f;
    public bool clockwise = false; //direction in which the door opens
    public bool isOpen = true;
    public bool isLocked = false;

    private float currentAngle = 0f;
    private float targetAngle = 0f;

    public void ChangeState()
    {
        if(!isLocked)
        {
            if(targetAngle == 0f)
                targetAngle = maxAngle * (clockwise ? 1 : -1);
            else
                targetAngle = 0f;
        }
    }

    void Start()
    {
        targetAngle = isOpen ? maxAngle * (clockwise ? 1 : -1) : 0f;
        currentAngle = targetAngle;
    }

    void Update()
    {
        transform.eulerAngles = Vector3.up * currentAngle;
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime);
    }
}
