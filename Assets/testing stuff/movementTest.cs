using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float speed = 1.25f, rotSpeed = 2f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Input.GetAxis("Vertical"), 0f, 0f);
        transform.Rotate(0f, 0f, Input.GetAxis("Mouse X") * rotSpeed);

        Debug.Log(Input.GetAxis("Vertical"));
    }
}
