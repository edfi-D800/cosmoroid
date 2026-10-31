using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S {get; private set;} // Singleton property

    [Header("Inscribed")]
    // These fields control the movement of the ship
    public float speed = 50f;
    //planning to implement pseudo-newtonian movement so i don't want roll/pitch here 
    //public float rollMult = -45;
    //public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 70;

    private Rigidbody rb;
    private float hAxis;
    private float vAxis;

    //[Header("Dynamic")] [Range(0,4)]
    //i want the ship to be one-shottable so i don't want a shield either
    //public float shieldLevel = 1;

    [Tooltip("This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;

    //Awake is called when the scene is first loaded in 
    void Awake() {
        if (S == null) {
            S = this; // Set the Singleton only if it's null
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }

        //cache Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Pull in information from the Input class
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        //opting for FixedUpdate for simulation-based movement
        //will still use Update for Input
        //Vector3 pos = transform.position;
        //pos.x += hAxis * speed * Time.deltaTime;
        //pos.y += vAxis * speed * Time.deltaTime;
        //transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        //planning to implement pseudo-newtonian movement so i don't want this part
        //transform.rotation = Quarternion.Euler(vAxis*pitchMult,hAxis*rollMult,0);

        // Allow the ship to fire
        if (Input.GetKeyDown(KeyCode.Space)) {
            TempFire();
        }
    }

    //temporary? projectile firing method
    void TempFire() {
        //get current WASD input
        float aimX = Input.GetAxisRaw("Horizontal"); //left (-1), right (1)
        float aimY = Input.GetAxisRaw("Vertical"); //down (-1), up (+1)

        //create direction vector from WASD input
        Vector3 aimDirection = new Vector3(aimX, aimY, 0f);

        //do not fire if no WASD keys are pressed
        //maybe implement dry clicking sound?
        if (aimDirection == Vector3.zero) {
            return;
        } 

        //instantiate and fire projectile
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        projGO.transform.rotation = Quaternion.LookRotation(Vector3.forward, aimDirection);
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        rigidB.linearVelocity = aimDirection * projectileSpeed;
    }

    void OnTriggerEnter(Collider other) {
        //If it's a hero bullet ignore it
        if (other.gameObject.layer == 10) return;

        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //Debug.Log("Player hit by: " +go.gameObject.name);

        // Make sure it's not the same triggering go as last time
        if(go == lastTriggerGo) return;
        //record lasTriggerGo as current go
        lastTriggerGo = go;
        Enemy enemy = go.GetComponent<Enemy>();

        //if collider trigger is an enemy, destroy the enemy
        //in all trigger cases describe what's being collided with
        if (enemy != null) {
            Destroy(go);
            Debug.Log("Player collided with: " +go.gameObject.name);
        } else {
            Debug.Log("Player hit with: " +go.gameObject.name);
        }

        //KILL
        Destroy(this.gameObject);
        //inform Main that there's No More Heroes
        Main.HERO_DIED();
    }

    //FixedUpdate called based on internal simulation time
    void FixedUpdate() {
        //apply physics forces with Vector3
        Vector3 movement = new Vector3(hAxis, vAxis, 0f);
        //normalize to prevent faster diagonal movement
        if (movement.magnitude > 1f) movement.Normalize();
        //apply newtonian thrust force (speed)
        rb.AddForce(movement * speed);
    }
}
