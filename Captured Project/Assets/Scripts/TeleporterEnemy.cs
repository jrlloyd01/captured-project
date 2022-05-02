using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterEnemy : MonoBehaviour
{
    public GameObject projectileEnemy;
    public bool active = false;
    public bool left = true;
    public BoundsCheck bndCheck;
    public float projectileSpeed = 10f;
    float health = 5f;
    float opacity = 1f;
    float elapsedTime = 0f;
    bool shot = false;
    GameObject hero;
    bool locationChosen = false;
    Vector3 location;
    bool disappeared = false;
    Color color;
    Rigidbody body;
    Material mat;
    GameObject lastTrigger = null;

    void Awake()
    {
        mat = this.GetComponent<MeshRenderer>().material;
        bndCheck = GetComponent<BoundsCheck>();
    }

    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        hero = GameObject.FindWithTag("Hero");
    }

    void Update()
    {
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
            if (this.transform.position.x > hero.transform.position.x)
            {
                left = true;
            }
            else
            {
                left = false;
            }

            if (elapsedTime >= 1f && !shot)
            {
                fire();
            }
            else if (elapsedTime >= 1f && shot)
            {
                teleport();
            }
        }
    }

    public void fire()
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
        shot = true;
    }

    public void teleport()
    {
        disappear();
        if (disappeared)
        {
            reappear();
        }

    }

    public void chooseLocation()
    {
        if (!locationChosen)
        {
            location = hero.transform.position;
            float locationX = location.x;
            float distanceAdder = Random.Range(7, bndCheck.camWidth);
            float num = Random.value;
            if (num > .5)
            {
                distanceAdder = -distanceAdder;
            }
            locationX = hero.transform.position.x + distanceAdder;
            location.x = locationX;
            this.transform.position = new Vector3(locationX, location.y, location.z);
            locationChosen = true;
            opacity = 0f;
            color = mat.color;
            color.a = opacity;
            mat.color = color;
        }
    }

    public void reappear()
    {
        //this.GetComponent<Collider>().enabled = true;
        opacity += Time.deltaTime * 2;
        Debug.Log(opacity);
        color = mat.color;
        if (opacity >= 1f)
        {
            opacity = 1f;
            elapsedTime = 0;
            shot = false;
            disappeared = false;
            locationChosen = false;
        }
        color.a = opacity;
        mat.color = color;
    }

    public void disappear()
    {
        //this.GetComponent<Collider>().enabled = false;
        opacity -= Time.deltaTime;
        if (opacity <= 0)
        {
            disappeared = true;
            opacity = 0;
            elapsedTime = 0;
            chooseLocation();
        }
        color = mat.color;
        color.a = opacity;
        mat.color = color;
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
