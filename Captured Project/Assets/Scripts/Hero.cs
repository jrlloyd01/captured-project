using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int health = 3;
    public float elapsedTimeShoot = 0f;
    public float movementSpeed = 5f;
    public float projectileSpeed = 30f;
    public float reloadTime = .5f;
    public float multiplier = 1f;
    public float jumpForce = 300f;
    public bool shotActive = false;
    public bool canJump = false;
    public bool canJumpChecked = false;
    public bool right = true;
    public GameObject projectile;
    public Rigidbody body;
    public bool flying = false;//if we add a flying/jetpack power-up
    public bool rapidFire = false;
    public bool shotgun = false;
    public bool invincible = false;
    public bool opacityDown = true;
    Material mat;
    Color color;
    public float opacity = 1f;

    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        mat = this.GetComponent<MeshRenderer>().material;
        reloadTime = .5f;
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        elapsedTimeShoot += Time.deltaTime;//to add limited fire rate
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector3 pos = transform.position;
        CheckJump();
        pos.x += xInput * movementSpeed * Time.deltaTime;
        if (flying)
        {
            canJump = false;
            body.useGravity = false;
            pos.y += yInput * movementSpeed * Time.deltaTime;
        }
        else
        {
            body.useGravity = true;
        }
        transform.position = pos;
        if (xInput > 0)//to determine shot direction and hero rotation
        {
            right = true;
        }
        else if (xInput < 0)
        {
            right = false;
        }
        if (right)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (!right)
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
            //not sure why y rotation causes horizontal rotation, but while testing this worked.
        }

        if (yInput != 0)//either up or down arrow
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && canJump && !flying)
            {
                jump();
                canJump = false;//for some reason will sometimes allow double jump when I was testing it. need to try to fix this.
                //may have something to do with velocity.y. May have to use onCollisionEnter() If(other.tag == "ground")
            }
            else if (flying)
            {
                pos.y += yInput * movementSpeed * Time.deltaTime;
                transform.position = pos;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))//button press
        {
            shotActive = true;
        }
        if (shotActive && !rapidFire)//button hold, no rapid fire
        {
            multiplier += Time.deltaTime;
        }
        else if (shotActive && rapidFire)//button hold, rapid fire
        {
            Fire();
        }
        if (Input.GetKeyUp(KeyCode.Space))//button release
        {
            if (multiplier > 2)
            {
                multiplier = 2f;
            }
            Fire();
            shotActive = false;
            //Debug.Log(multiplier);
        }
        invincibilityFlash();
    }

    public void jump()
    {
        if (canJump)
        {
            body.AddForce(0, jumpForce, 0);
            canJump = false;
        }

    }

    void Fire()
    {
        if (elapsedTimeShoot >= reloadTime)
        {
            if (!rapidFire && !shotgun)
            {
                GameObject projGO = Instantiate(projectile);
                /*float multiplierScaleX = projectile.transform.localScale.x * multiplier;
                float multiplierScaleY = projectile.transform.localScale.y * multiplier;
                float multiplierScaleZ = projectile.transform.localScale.z * multiplier;
                projGO.transform.localScale = new Vector3(multiplierScaleX, multiplierScaleY, multiplierScaleZ);*/
                projGO.GetComponent<ProjectileHero>().setDamage(multiplier);
                projGO.transform.position = transform.position;
                Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
                if (right)
                {
                    rigidB.velocity = Vector3.right * projectileSpeed;
                }
                else
                {
                    rigidB.velocity = Vector3.left * projectileSpeed;
                }
                elapsedTimeShoot = 0;
            }
            else if (rapidFire && !shotgun)
            {
                GameObject projGO = Instantiate(projectile);
                projGO.transform.position = transform.position;
                Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
                if (right)
                {
                    rigidB.velocity = Vector3.right * projectileSpeed;
                }
                else
                {
                    rigidB.velocity = Vector3.left * projectileSpeed;
                }
                elapsedTimeShoot = 0;
            }
            else if (shotgun && !rapidFire)
            {
                GameObject projGO;
                /*float multiplierScaleX = projectile.transform.localScale.x * multiplier;
                float multiplierScaleY = projectile.transform.localScale.y * multiplier;
                float multiplierScaleZ = projectile.transform.localScale.z * multiplier;*/
                float yProj = projectileSpeed / 4;
                for (int i = 0; i < 5; i++)
                {
                    projGO = Instantiate(projectile);
                    //projGO.transform.localScale = new Vector3(multiplierScaleX, multiplierScaleY, multiplierScaleZ);
                    projGO.GetComponent<ProjectileHero>().setDamage(multiplier);
                    projGO.transform.position = transform.position;
                    Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
                    if (right)
                    {
                        rigidB.velocity = new Vector3(projectileSpeed, yProj, 0);
                    }
                    else
                    {
                        rigidB.velocity = new Vector3(-projectileSpeed, yProj, 0);
                    }
                    yProj -= (projectileSpeed / 8);
                    elapsedTimeShoot = 0;
                }
            }
            else if (shotgun && rapidFire)
            {
                GameObject projGO;
                float yProj = projectileSpeed / 4;
                for (int i = 0; i < 5; i++)
                {
                    projGO = Instantiate(projectile);
                    projGO.transform.position = transform.position;
                    Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
                    if (right)
                    {
                        rigidB.velocity = new Vector3(projectileSpeed, yProj, 0);
                    }
                    else
                    {
                        rigidB.velocity = new Vector3(-projectileSpeed, yProj, 0);
                    }
                    yProj -= (projectileSpeed / 8);
                    elapsedTimeShoot = 0;
                }

            }
            elapsedTimeShoot = 0;
        }
        multiplier = 1;
    }
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        canJumpChecked = false;
        //Invoke("CheckJump", .15f);
        /*if(other.tag =="ground")
        {
            Debug.Log("ground collision");
            this.canJump = true;
        }*/
        if (other.tag == "Enemy" || other.tag == "ProjectileEnemy")
        {
            if (!invincible)
            {
                health--;
                invincible = true;
                Invoke("resetInvincible", 1f);
                if (health == 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {

            }
        }
        else if (other.tag == "PowerUpS")
        {
            shotgun = true;
            Destroy(other);
        }
        else if (other.tag == "PowerUpR")
        {
            rapidFire = true;
            reloadTime = reloadTime / 3;
        }

    }

    void resetInvincible()
    {
        invincible = false;
        opacityDown = true;
        opacity = 1f;
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            color = child.GetComponent<MeshRenderer>().material.color;
            color.a = opacity;
            child.GetComponent<MeshRenderer>().material.color = color;
        }

    }

    void CheckJump()
    {
        if (!canJumpChecked && body.velocity.y > -.1 && body.velocity.y < .1)
        {
            canJump = true;
            canJumpChecked = true;
        }
    }

    void invincibilityFlash()
    {
        if (invincible)
        {
            
            if (opacityDown)
            {
                opacity -= Time.deltaTime * 5;
            }
            else
            {
                opacity += Time.deltaTime * 5;
            }
            if (opacity <= 0)
            {
                opacity = 0;
                opacityDown = false;
            }
            if (opacity >= 1f)
            {
                opacity = 1;
                opacityDown = true;
            }
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                color = child.GetComponent<MeshRenderer>().material.color;
                color.a = opacity;
                child.GetComponent<MeshRenderer>().material.color = color;
            }

        }
    }
}
