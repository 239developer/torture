using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Rigidbody player;
    public float shakeIntensity = 1.0f;
    public float shakeTime = 1f;

    private float lastShake = 0.0f;
    private float timer = 0.0f;
    private bool fromZero = true;
    private Vector3 nextPosition;

    Vector3 GetNewPosition()
    {
        return 0.5f * shakeIntensity * new Vector3(Random.value - 0.5f, 1f, 0f);
    }

    void Start()
    {
        nextPosition = GetNewPosition();
    }

    void Update()
    {
        if(timer - lastShake > shakeTime)
        {
            lastShake = timer;
            if(!fromZero)
                nextPosition = GetNewPosition();
            fromZero = !fromZero;
        }

        Vector3 a = fromZero ? Vector3.zero : nextPosition;
        Vector3 b = fromZero ? nextPosition : Vector3.zero;
        float t = (timer - lastShake) / shakeTime;
        transform.localPosition = Vector3.Slerp(a, b, t);

        if(player.velocity.x != 0f || player.velocity.z != 0)
            timer += Time.deltaTime;
    }
}
