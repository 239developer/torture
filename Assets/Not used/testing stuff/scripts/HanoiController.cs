using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiController : MonoBehaviour
{
    public float poleHeight = 2f;
    public float moveTime = 0.8f;
    public float armLength = 1.0f;

    public GameObject[] disks; //smol to bigg
    public GameObject[] poleInstances;
    public Vector3[] origins;
    public GameObject[] slots;

    private GameObject cam;
    private GameObject held = null;
    private int disabled = 1;

    private float[] lastChange = new float[4];
    private float resetTime = 0f;
    private bool reset = false;

    private static int[][] nullgrid =   //top to bottom
        {new int[]{1, 2, 3, 4, 5, 6, 7, 8},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0}};
    private int[][] grid =
        {new int[]{1, 2, 3, 4, 5, 6, 7, 8},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0},
        new int[]{0, 0, 0, 0, 0, 0, 0, 0}};

    int FindHeightWithSize(int size, int pole)
    {
        int h = -1;
        for(int i = size - 1; i < 8; i++)
            if(grid[pole][i] == size)
            {
                h = i;
                break;
            }
        return h;
    }

    void Move()
    {
        slots[disabled].GetComponent<SlotOpener>().Open();
        lastChange[disabled] = Time.time;
        if(++disabled == 4)
            disabled = 0;
        slots[disabled].GetComponent<SlotOpener>().Close();
        lastChange[disabled] = Time.time;
    }

    GameObject TryToPickUpFrom(int id)
    {
        int smallestDisk = 0;
        for(int i = 0; i < 8; i++)
            if(smallestDisk == 0)
                smallestDisk = grid[id][i];

        if(smallestDisk == 0) return null;
        else return disks[smallestDisk - 1];
    }

    void TryToPutOn(int id, int size, int from)
    {
        size++;
        int pos = -1;
        if(from == id)
            pos = FindHeightWithSize(size, from);
        else if(grid[id][7] != 0)
            for(int i = 1; i < 8; i++)
            {
                if(size < grid[id][i])
                {
                    pos = i - 1;
                    break;
                }
                if(grid[id][i] != 0)
                    break;
            }
        else pos = 7;

        if(pos >= 0 || id == from)
        {
            int h = FindHeightWithSize(size, from);
            grid[from][h] = 0;
            grid[id][pos] = size;
            held.transform.position = origins[id] + Vector3.up * 0.2f * (7 - pos);
            held.transform.parent = poleInstances[id].transform;
            held = null;
            if(id != from) Move();
        }
    }

    void Reset()
    {
        for(int i = 0; i < 4; i++)
            if(i != disabled)
            {
                lastChange[i] = Time.time;
                slots[i].GetComponent<SlotOpener>().Close();
            }
        for(int i = 0; i < 4; i++)
            grid[i] = (int[])nullgrid[i].Clone();
        Debug.Log(grid[0][0]);
        reset = true;
        resetTime = Time.time + 1.0f;
    }

    void Start()
    {
        cam = GameObject.Find("Player Camera");
        slots[disabled].GetComponent<SlotOpener>().Close();

        for(int i = 0; i < 4; i++)
            grid[i] = (int[])nullgrid[i].Clone();
    }

    void Update()
    {
        int camX = cam.GetComponent<Camera>().pixelWidth;
        int camY = cam.GetComponent<Camera>().pixelHeight / 2;
        Vector3 pos = new Vector3(camX / 2, camY, 0f);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(pos);
        Debug.DrawRay(cam.transform.position, ray.direction * 1, Color.red);

        if(Input.GetKeyDown(KeyCode.E) && !reset)
        {
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, ray.direction, out hit, Mathf.Infinity, 3 << 8))
            {
                if(hit.collider.gameObject.layer == 9)
                {
                    char c = hit.collider.name.ToCharArray()[5];
                    int id = c - '0';
                
                    if(id != disabled)
                    {
                        if(held == null)
                            held = TryToPickUpFrom(id);
                        else
                        {
                            char ch = held.transform.parent.name.ToCharArray()[0];
                            TryToPutOn(id, System.Int32.Parse(held.name), ch - '0');
                        }
                    }
                }
                else if(hit.collider.name == "reset")
                    Reset();
            }
        }

        if(held != null)
        {
            held.transform.position = cam.transform.position + ray.direction * armLength;
        }

        //moving poles
        for(int i = 0; i < 4; i++)
        {
            float t = (Time.time - lastChange[i]) / moveTime;
            t = i == disabled || reset ? t : 1f - t;
            Vector3 p = Vector3.Lerp(Vector3.zero, Vector3.down * poleHeight, t);
            poleInstances[i].transform.position = p;
        }
        if(Time.time >= resetTime && reset)
        {
            disabled = 1;
            for(int i = 0; i < 4; i++)
                if(i != disabled)
                {
                    lastChange[i] = Time.time;
                    slots[i].GetComponent<SlotOpener>().Open();
                }
            slots[disabled].GetComponent<SlotOpener>().Close();
            held = null;
            for(int i = 0; i < 8; i++)
            {
                disks[i].transform.parent = poleInstances[0].transform;
                disks[i].transform.position = origins[0] + Vector3.up * (0.2f * (7 - i) - poleHeight);
            }
            reset = false;
        }
    }
}
