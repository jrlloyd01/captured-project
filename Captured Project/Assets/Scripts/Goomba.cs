using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public bool active = false;
    public bool moveActive = false;
    public bool left = true;
    public BoundsCheck bndCheck;
    public float movementSpeed = 5f;
    public float health = 3f;
    public bool changeDirection = true;
    Rigidbody body;
    GameObject lastTrigger = null;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Start()
    {
        body = this.GetComponent<Rigidbody>();
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
        }
    }

    public void move()
    {
        if (moveActive)
        {
            if (left)
            {
                Vector3 pos = this.transform.position;
                pos.x -= movementSpeed * Time.deltaTime;
                this.transform.position = pos;
            }
            else
            {
                Vector3 pos = this.transform.position;
                pos.x += movementSpeed * Time.deltaTime;
                this.transform.position = pos;
            }
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

        if (other.tag != "Hero" && other.tag != "ProjectileHero")
        {
            if (moveActive && changeDirection && (body.velocity.y != 0))
            {
                left = !left;
            }
            else
            {
                moveActive = true;
            }
        }
        else if (other.tag == "Hero")
        {
            changeDirection = false;
            Invoke("resetChangeDirection", 2f);
        }
        else if (other.tag == "ProjectileHero")
        {
            changeDirection = false;
            Invoke("resetChangeDirection", 2f);
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

    void resetChangeDirection()
    {
        changeDirection = true;
    }
}
