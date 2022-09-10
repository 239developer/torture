﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.0f;
    public float acceleration = 1f;
    public float sprintDuration = 7.5f;
    public float sprintSpeedMultiplier = 2.0f;
    public float sprintCDSpeed = 0.5f;
    public float minStamina = 0.3f;
    public float maxCameraTiltUp = 5f;
    public float maxCameraTiltDown = 45f;

    private Rigidbody rb;
    private GameObject camera_;
    private float stamina = 0f;
    private bool isSprinting = false;

    void ApplyVelocity(float x, float z)
    {
        //applying player velocity
        Vector3 v = rb.velocity;
        Vector3 transformed = transform.TransformVector(new Vector3(x, 0f, z));
        x = transformed.x;
        z = transformed.z;
        Vector3 u = new Vector3(x, v.y, z);
        rb.velocity = u;
    }

    void ApplyPlayerRotation(float angle)
    {
        //player rotation (left/right)
        transform.Rotate(Vector3.up, angle, Space.World);
    }

    void ApplyCameraRotation(float angle)
    {
        //camera rotaion(looking up/down)

        Vector3 axis = camera_.transform.TransformVector(Vector3.right);
        Quaternion currentRotation = camera_.transform.rotation;
        Quaternion maxRotation = Quaternion.AngleAxis(-maxCameraTiltUp, axis) * transform.rotation;
        Quaternion minRotation = Quaternion.AngleAxis(maxCameraTiltDown, axis) * transform.rotation;

        float alpha = Quaternion.Angle(currentRotation, minRotation);
        float beta = Quaternion.Angle(currentRotation, maxRotation);
        float currentAngle = 0f;
        if(alpha > beta)
        {
            currentAngle = beta - maxCameraTiltUp;
            if(currentAngle + angle < -maxCameraTiltUp)
                camera_.transform.rotation = maxRotation;
            else
                camera_.transform.Rotate(Vector3.right, angle, Space.Self);
        }
        else
        {
            currentAngle = maxCameraTiltDown - alpha;
            if(currentAngle + angle > maxCameraTiltDown)
                camera_.transform.rotation = minRotation;
            else
                camera_.transform.Rotate(Vector3.right, angle, Space.Self);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        // rb.interpolation = RigidbodyInterpolation.Extrapolate;
        camera_ = GetComponentInChildren<Camera>().gameObject;
    }

    void Update()
    {
        //Sprint check
        if((stamina > minStamina || (stamina > 0f && isSprinting)) && Input.GetKey("left shift"))
        {
            isSprinting = true;
            stamina -= Time.deltaTime / sprintDuration;
        }
        else
        {
            isSprinting = false;
            if(stamina < 1f)
                stamina += Time.deltaTime * sprintCDSpeed;
            else stamina = 1f;
        }

        //Rotation
        float sensivity = Settings.sensivity;
        float alpha = Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        float beta = -Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime * 0.5625f;

        ApplyPlayerRotation(alpha);
        ApplyCameraRotation(beta);
    }

    void FixedUpdate()
    {
        //Movement
        float dx = Input.GetAxis("Horizontal") * speed;
        dx *= isSprinting ? sprintSpeedMultiplier : 1;
        float dz = Input.GetAxis("Vertical") * speed;
        dz *= isSprinting ? sprintSpeedMultiplier : 1;

        ApplyVelocity(dx, dz);
    }
}
