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
    private bool scoreAdded = false;
    GameObject hero;
    bool locationChosen = false;
    Vector3 location;
    bool disappeared = false;
    Color color;
    //Rigidbody body;
    //Material mat;
    GameObject lastTrigger = null;

    void Awake()
    {
        //mat = this.GetComponent<MeshRenderer>().material;
        bndCheck = this.GetComponent<BoundsCheck>();
    }

    void Start()
    {
        if(bndCheck == null)
        {
            //Debug.Log("bndCheck is NULL");
            bndCheck = this.GetComponent<BoundsCheck>();
        }
        //body = this.GetComponent<Rigidbody>();
        hero = GameObject.FindWithTag("Hero");
        opacity = 1f;
    }

    void Update()
    {
        if (hero.transform.position.x <= this.transform.position.x)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            left = true;
        }
        else
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
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
            if (elapsedTime >= 1.5f && !shot)
            {
                fire();
                //Debug.Log("Shot");
            }
            else if (elapsedTime >= 1.5f && shot)
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
        }
    }

    public void reappear()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.GetComponent<Collider>().enabled = true;
        }
        this.GetComponent<Collider>().enabled = true;

        opacity += Time.deltaTime * 2;
        //Debug.Log(opacity);
        if (opacity >= 1f)
        {
            opacity = 1f;
            elapsedTime = 0;
            shot = false;
            disappeared = false;
            locationChosen = false;
        }

        foreach (Transform child in allChildren)
        {
            color = child.GetComponent<MeshRenderer>().material.color;
            color.a = opacity;
            child.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    public void disappear()
    {
        Transform[] allChildren = this.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            child.gameObject.GetComponent<Collider>().enabled = false;
        }
        this.GetComponent<Collider>().enabled = false;
        opacity -= Time.deltaTime;
        if (opacity <= 0)
        {
            disappeared = true;
            opacity = 0;
            elapsedTime = 0;
            chooseLocation();
        }
        
        foreach (Transform child in allChildren)
        {
            //Debug.Log("Children");
            if(child.GetComponent<MeshRenderer>().material.color == null)
            {
                //Debug.Log("doesn't contain color");
            }
            color = child.GetComponent<MeshRenderer>().material.color;
            color.a = opacity;
            child.GetComponent<MeshRenderer>().material.color = color;
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
            health = health - damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                if (!scoreAdded)
                {
                    hero.GetComponent<Hero>().increaseScore(300);
                    scoreAdded = true;
                }
            }
            Destroy(other);
        }
    }
}
