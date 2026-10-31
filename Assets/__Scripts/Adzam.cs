using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adzam extends Enemy class
public class Adzam : Enemy
{
    [Header("Adzam Inscribed Fields")]

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Tooltip("Time in seconds between angle changes")]
    public float changeInterval = 15f;
    
    private Vector3 direction;
    private float timeUntilNextChange;
    private bool hasFiredThisCycle = false;
    private Collider adzamCollider;

    //Awake is called when the scene is first loaded in
    void Start() {
        //pick a random direction
        GetNewRandomDirection();
        adzamCollider = GetComponent<Collider>();
    }

    // Override the move function on Enemy
    public override void Move() {
        //reduce timer 
        timeUntilNextChange -=  Time.deltaTime;

        //if timer is below half of interval and this enemy hasn't fired yet, fire
        if((!hasFiredThisCycle) && (timeUntilNextChange <= (changeInterval/2f))) {
            StartCoroutine(FireBurstRoutine());
            hasFiredThisCycle = true; //prevents fire until next direction change
        }

        //if timer hits zero, pick new direction and reset time
        if (timeUntilNextChange <= 0f) {
            GetNewRandomDirection();
        }

        //move current position based on direction and speed values
        Vector3 tempPos = pos;
        tempPos += direction * speed * Time.deltaTime;
        pos = tempPos;

        //base.Move();

        //print(bndCheck.isOnScreen); // Leave this line commented out
    }

    //coroutine that handles 3-shot burst independently of Move()
    private IEnumerator FireBurstRoutine() {
        int burstCount = 3;
        float delayBetweenShots = 0.20f;

        for (int i = 0; i < burstCount; i++) {
            targetFire();

            if (i < (burstCount-1)) {
                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
    }

    void GetNewRandomDirection()
    {
        //pick random angle in radians
        float randomAngle = Random.Range(0f, Mathf.PI*2);

        //convert angle into a Vector3 direction
        direction = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);

        //reset countdown timer
        timeUntilNextChange = changeInterval;

        //reset firing flag
        hasFiredThisCycle = false;
    }

    //fire in direction of hero
    void targetFire() {
        //locate hero
        GameObject hero = GameObject.FindWithTag("Player");

        //check if hero is null
        if (hero == null) {
            return;
        }
        //calculate direction from target position - current position
        Vector3 aimDirection = hero.transform.position - transform.position;
        aimDirection.z = 0f;
        aimDirection.Normalize();

        //instantiate projectile
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);

        //have zaku inject itself as projectile owner
        if (projGO.TryGetComponent<ProjectileEnemy>(out ProjectileEnemy proj)) {
            proj.Initialize(this.gameObject);
        }

        //set projectile movement
        projGO.transform.position = transform.position;
        projGO.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDirection);
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.linearVelocity = aimDirection * projectileSpeed;
    }

    //override enemy OnCollisionEnter() so that adzam is invulnerable to player attacks
    public override void OnCollisionEnter(Collision coll) {
        //get the other GameObject in the collision
        GameObject otherGO = coll.gameObject;

        //check if this is enemy's own bullet, ignore if so
        if (coll.gameObject.TryGetComponent<ProjectileEnemy>(out ProjectileEnemy proj)) {
            if (proj.Owner == this.gameObject) {
                return;
            }
        }

        //if projectile is ProjectileHero, destroy it
        if (otherGO.GetComponent<ProjectileHero>() != null) {
            Destroy(otherGO);
        }
        //if it has one of two projectile classes attached, destroy both it and this Enemy object
        if ((otherGO.GetComponent<ProjectileEnemy>() != null)) {
            Destroy(otherGO);
            health -= 1;
            Debug.Log(this.gameObject.name + " has remaining health " + health);
        } 
        //if health has reached below 0, KILL
        if (health <= 0) {
            ScoreCounter.Instance.AddScore(score);
            Destroy(gameObject);
            Debug.Log(this.gameObject.name + "has been splashed!");
        }
        //optional else for debugging purposes not implemented
    }
}
