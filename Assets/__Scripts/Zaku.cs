using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zaku extends Enemy class
public class Zaku : Enemy
{
    [Header("Zaku Inscribed Fields")]

    public GameObject projectilePrefab;
    public float projectileSpeed = 40;

    [Tooltip("Time in seconds between angle changes")]
    public float changeInterval = 5f;
    
    private Vector3 direction;
    private float timeUntilNextChange;
    private bool hasFiredThisCycle = false;
    private Collider zakuCollider;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;

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
            randomFire();
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

    //fire in random direction
    void randomFire() {
        //generate random angle in radians
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        //create direction vector from angle
        float aimX = Mathf.Cos(randomAngle);
        float aimY =Mathf.Sin(randomAngle);
        Vector3 aimDirection = new Vector3(aimX, aimY, 0f);

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
        PlayShootSound();
    }

    //sfx method for shooting
    public void PlayShootSound() {
        if ((audioSource != null) && (shootClip != null)) {
            audioSource.PlayOneShot(shootClip);
        }
    }
}
