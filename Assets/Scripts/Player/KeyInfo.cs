using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInfo : MonoBehaviour
{
    public int id = 0;

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
