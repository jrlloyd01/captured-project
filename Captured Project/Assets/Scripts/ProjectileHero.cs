using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;
    static public ProjectileHero S;
    public float damage = 1f;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public void setDamage(float multiplier)
    {
        damage = damage * multiplier;
        float multiplierScaleX = this.transform.localScale.x * multiplier;
        float multiplierScaleY = this.transform.localScale.y * multiplier;
        float multiplierScaleZ = this.transform.localScale.z * multiplier;
        this.transform.localScale = new Vector3(multiplierScaleX, multiplierScaleY, multiplierScaleZ);
    }

    void Update()
    {
        if (!bndCheck.isOnScreen)
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
