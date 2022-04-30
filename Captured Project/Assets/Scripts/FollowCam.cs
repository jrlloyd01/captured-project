using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    [Header("Set Dynamically")]
    public float camZ;
    [Header("Set in Inspector")]
    public float easing = .05f;
    void Awake()
    {
        if (POI == null)
        {
            POI = GameObject.FindWithTag("Hero");
        }
        camZ = transform.position.z;
    }
    void FixedUpdate()
    {
        Vector3 destination;
        destination = POI.transform.position;
        destination.y = Mathf.Max(0, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
    }
}
