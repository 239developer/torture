using System;
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
    public float minStamina = 0.1f;

    private Rigidbody rb;
    private float stamina = 0f;
    private bool isSprinting = false;

    void ApplyMovement(float x, float z)
    {
        
        Vector3 v = rb.velocity;
        Vector3 transformed = transform.TransformVector(new Vector3(x, 0f, z));
        x = transformed.x;
        z = transformed.z;
        Vector3 u = new Vector3(x, v.y, z);
        rb.velocity = u;
    }

    void ApplyPlayerRotation(float angle)
    {

    }

    void ApplyCameraRotation(float angle)
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.interpolation = RigidbodyInterpolation.Extrapolate;
    }

    // void Update()
    // {
    //     //Sprint check
    //     if(stamina > minStamina && Input.GetKey("left shift"))
    //     {
    //         isSprinting = true;
    //         stamina -= Time.deltaTime / sprintDuration;
    //     }
    //     else
    //     {
    //         isSprinting = false;
    //         if(stamina < 1f)
    //             stamina += Time.deltaTime * sprintCDSpeed;
    //         else stamina = 1f;
    //     }
    // }

    void FixedUpdate()
    {
        //Movement
        float dx = Input.GetAxis("Horizontal") * speed;
        dx *= isSprinting ? sprintSpeedMultiplier : 1;
        float dz = Input.GetAxis("Vertical") * speed;
        dz *= isSprinting ? sprintSpeedMultiplier : 1;

        ApplyMovement(dx, dz);

        //Rotation
        float sensivity = Settings.sensivity;
        float alpha = Input.GetAxis("Mouse X") * sensivity;
        float beta = Input.GetAxis("Mouse Y") * sensivity;

        ApplyPlayerRotation(alpha);
        ApplyCameraRotation(beta);
    }
}
