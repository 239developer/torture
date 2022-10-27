using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectingStuff : MonoBehaviour
{
    public float holdingDistance = 0.6f;
    public float grabDistance = 3f;
    public GameObject cameraPosition;

    private GameObject heldObject = null;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Rigidbody rb;

    private const string holdableTag = "Holdable";
    private const string doorTag = "Door";

    private GameObject GetObjectFromRaycast(out float distance)
    {
        Vector3 origin = cameraPosition.transform.position;
        Vector3 direction = cameraPosition.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(origin, direction, out hit))
        {
            distance = hit.distance;
            return hit.collider.gameObject;
        }
        distance = -1f;
        return null;
    }

    void Update()
    {
        if(heldObject == null && Input.GetKeyDown(KeyCode.E))
        {
            float distance;
            GameObject obj = GetObjectFromRaycast(out distance);
            switch(obj.tag)
            {
                case holdableTag:
                    heldObject = distance < grabDistance ? obj : null;
                    if(heldObject != null)
                    {
                        originalPosition = heldObject.transform.position;
                        originalRotation = heldObject.transform.rotation;
                        PlayerController.isFrozen = true;
                        Time.timeScale = 0f;
                    }
                    break;
                case doorTag:
                    obj.GetComponentInParent<DoorController>().ChangeState();
                    break;
            }
        }
        else if(heldObject != null)
        {
            rb = heldObject.GetComponent<Rigidbody>();

            Vector3 expectedPosition = cameraPosition.transform.forward * holdingDistance + cameraPosition.transform.position;
            Vector3 v = expectedPosition - heldObject.transform.position;
            heldObject.transform.position += v;

            Vector3 forward = -1 * cameraPosition.transform.forward;
            Vector3 up = cameraPosition.transform.up;
            heldObject.transform.rotation = Quaternion.LookRotation(forward, up);
        
            if(Input.GetKeyDown(KeyCode.E))
            {
                heldObject.transform.position = originalPosition;
                heldObject.transform.rotation = originalRotation;
                heldObject = null;
                PlayerController.isFrozen = false;
                Time.timeScale = 1f;
            }
        }
    }
}
