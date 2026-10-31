using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))] //tells Unity that Enemy class must have a BoundsCheck component
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f; //the movement speed (in m/s)
    public float fireRate = 0.3f; // Seconds/shot (Unused)
    public float health = 1; // Damage needed to destroy this enemy
    public int score = 100; // Points earned for destroying this enemy
    protected BoundsCheck bndCheck; //stores a reference to this object's BoundsCheck component

    public ScoreCounter scoreCounter; //stores reference to script in ScoreCounter object

    //Awake is called when the scene is first loaded in 
    void Awake() {
        bndCheck = GetComponent<BoundsCheck>(); //assign this object's BoundsCheck component to bndCheck
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos {
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //movement method
        Move();
        //if enemy has exceeded bottom screen border, destroy it
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown)) {
            Destroy(gameObject);
        }
    }

    //gets current position of this GameObject, moves it in the downward Y direction, assigs it back to pos
    public virtual void Move() {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    public virtual void OnCollisionEnter(Collision coll) {
        //get the other GameObject in the collision
        GameObject otherGO = coll.gameObject;

        //check if this is enemy's own bullet, ignore if so
        if (coll.gameObject.TryGetComponent<ProjectileEnemy>(out ProjectileEnemy proj)) {
            if (proj.Owner == this.gameObject) {
                return;
            }
        }

        //if it has one of two projectile classes attached, destroy both it and this Enemy object
        if ((otherGO.GetComponent<ProjectileHero>() != null) || (otherGO.GetComponent<ProjectileEnemy>() != null)) {
            ScoreCounter.Instance.AddScore(score);
            Destroy(otherGO);
            Destroy(gameObject);
        } 
        //optional else for debugging purposes not implemented
    }
}
