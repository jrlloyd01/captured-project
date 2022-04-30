using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float radius = 2f;
    static public GameObject POI;

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;
    public bool isOnScreen;

    [Header("Hide In Inspector")]
    public bool offRight, offLeft, offUp, offDown;

    void Awake()
    {
        if (POI == null)
        {
            POI = GameObject.FindWithTag("Hero");
        }
        camHeight = Camera.main.orthographicSize;
        camWidth = (camHeight * Camera.main.aspect) + 1;
    }
    void LateUpdate()
    {

        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if ((this.transform.position.x - POI.transform.position.x) > camWidth)
        {
            offRight = true;
        }
        if ((this.transform.position.x - POI.transform.position.x) < -camWidth)
        {
            offLeft = true;
        }
        if ((this.transform.position.y - POI.transform.position.y) > camHeight)
        {
            offUp = true;
        }
        if ((this.transform.position.y - POI.transform.position.y) < -camHeight)
        {
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, .1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
