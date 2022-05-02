using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollwerEnemy : MonoBehaviour
{
    public bool active = false;
    public bool moveActive = false;
    public bool left = true;
    public BoundsCheck bndCheck;
    public float movementSpeed = 2.5f;
    public float health = 3f;
    Rigidbody body;
    GameObject hero;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Start()
    {
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
            move();
            if (hero.transform.position.x <= this.transform.position.x)
            {
                this.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }

    public void move()
    {
        Vector3 destination = hero.transform.position;
        if (destination.x < hero.transform.position.x)
        {
            destination.x += (movementSpeed * Time.deltaTime);
        }
        else if (destination.x > hero.transform.position.x)
        {
            destination.x -= (movementSpeed * Time.deltaTime);
        }
        transform.position = destination;
    }

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        if (other.tag == "ProjectileHero")
        {
            float damage = other.GetComponent<ProjectileHero>().damage;
            Debug.Log(damage);
            health = health - damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                //score += 100;
            }
            Destroy(other);
        }
    }
}
