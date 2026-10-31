using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Red Zaku extends Enemy class
public class RedZaku : Enemy
{
    [Header("Zaku Inscribed Fields")]

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Tooltip("Time in seconds between angle changes")]
    public float changeInterval = 2f;
    
    private Vector3 direction;
    private float timeUntilNextChange;
    private bool hasFiredThisCycle = false;
    private Collider zakuCollider;

    //Awake is called when the scene is first loaded in
    void Start() {
        //pick a random direction
        GetNewRandomDirection();
        zakuCollider = GetComponent<Collider>();
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
}
