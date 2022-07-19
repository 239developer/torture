using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementTest : MonoBehaviour
{
    public float speed = 1.25f, rotSpeed = 2f;
    public GameObject cam;
    private Quaternion startRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startRotation = cam.transform.rotation;
    }

    void Update()
    {
        float dx = speed * Time.deltaTime * Input.GetAxis("Vertical");
        float dy = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        transform.Translate(dx, dy, 0f);
        transform.Rotate(0f, 0f, Input.GetAxis("Mouse X") * rotSpeed);

        cam.transform.Rotate(-rotSpeed * Input.GetAxis("Mouse Y"), 0f, 0f);
    }
}
