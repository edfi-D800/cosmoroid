using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Zaku extends Enemy class
public class Zaku : Enemy
{
    [Header("Zaku Inscribed Fields")]
    [Tooltip("Time in seconds between angle changes")]
    public float changeInterval = 5f;
    
    private Vector3 direction;
    private float timeUntilNextChange;

    void Start() {
        //pick a random direction
        GetNewRandomDirection();
    }

    // Override the move function on Enemy
    public override void Move() {
        //reduce timer 
        timeUntilNextChange -=  Time.deltaTime;

        //if timer hits zero, pick new direction and reset time
        if (timeUntilNextChange <= 0f) {
            GetNewRandomDirection();
        }

        //move current position based on direction and speed values
        Vector3 tempPos = pos;
        tempPos += direction * speed * Time.deltaTime;
        pos = tempPos;

        //base.Move();

        // print(bndCheck.isOnScreen); // Leave this line commented out
    }

    void GetNewRandomDirection()
    {
        //pick random angle in radians
        float randomAngle = Random.Range(0f, Mathf.PI*2);

        //convert angle into a Vector3 direction
        direction = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);

        //reset countdown timer
        timeUntilNextChange = changeInterval;
    }
}
