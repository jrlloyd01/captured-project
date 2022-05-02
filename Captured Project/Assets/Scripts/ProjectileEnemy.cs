using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    private BoundsCheck bndCheck;
    static public ProjectileEnemy S;
    GameObject hero;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        hero = GameObject.FindWithTag("Hero");
    }

    void Update()
    {
        if ((this.transform.position.x - hero.transform.position.x) > (bndCheck.camWidth * 2) || (this.transform.position.x - hero.transform.position.x) < -bndCheck.camWidth * 2)
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        Destroy(this.gameObject);
    }
}
