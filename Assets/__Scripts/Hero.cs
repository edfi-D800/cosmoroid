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

    private Rigidbody rb;
    private float hAxis;
    private float vAxis;

    //[Header("Dynamic")] [Range(0,4)]

    //i want the ship to be one-shottable so i don't want a shield either
    //public float shieldLevel = 1;

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

    //Please delete the unused Start() method
}
