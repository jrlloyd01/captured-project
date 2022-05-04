using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    public bool active = false;
    public bool moveActive = false;
    public bool left = true;
    public BoundsCheck bndCheck;
    public float movementSpeed = 5f;
    public float health = 3f;
    public GameObject projectileEnemy;
    public float projectileSpeed = 10f;
    float elapsedTime = 0;
    Rigidbody body;
    GameObject hero;
    GameObject lastTrigger = null;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Start()
    {
        Debug.Log(health);
        body = this.GetComponent<Rigidbody>();
        hero = GameObject.FindWithTag("Hero");
    }

    void Update()
    {
        if (!active)
        {
            if (bndCheck.isOnScreen)
            {
                active = true;
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;
            if (hero.transform.position.x <= this.transform.position.x)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
                left = true;
            }
            else
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
                left = false;
            }
            if (elapsedTime > 3f)
            {
                fire();
                elapsedTime = 0;
            }
        }
    }

    void fire()
    {
        GameObject projGO = Instantiate(projectileEnemy);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        if (!left)
        {
            rigidB.velocity = Vector3.right * projectileSpeed;
        }
        else
        {
            rigidB.velocity = Vector3.left * projectileSpeed;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        if (other == lastTrigger)
        {
            return;
        }
        else
        {
            lastTrigger = other;
        }

        if (other.tag == "Hero")
        {

        }
        else if (other.tag == "ProjectileHero")
        {
            float damage = other.GetComponent<ProjectileHero>().damage;
            health -= damage;
            Debug.Log("Health: " + health);
            if (health <= 0)
            {
                Destroy(this.gameObject);
                //score += 100;
            }
            Destroy(other);
        }
    }
}
