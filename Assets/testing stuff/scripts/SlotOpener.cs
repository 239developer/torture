using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotOpener : MonoBehaviour
{
    public bool opened = true;
    public float moveTime = 0.8f;
    public float delay = 0.0f;
    public GameObject[] part;
    public Vector3[] openedPos;
    public Vector3[] closedPos;

    private float lastChange = 0f;

    public void Open()
    {
        if(!opened) lastChange = Time.time + delay;
        opened = true;
    }

    public void Close()
    {
        if(opened) lastChange = Time.time;
        opened = false;
    }

    void Update()
    {
        for(int i = 0; i < 2; i++)
        {
            float t = (Time.time - lastChange) / moveTime;
            t = opened ? t : 1f - t;
            Vector3 pos = Vector3.Lerp(closedPos[i], openedPos[i], t);
            part[i].transform.position = pos;
        }
    }
}
