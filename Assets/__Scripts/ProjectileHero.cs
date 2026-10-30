using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{
    private BoundsCheck bndCheck;

    //Awake is called when the scene is first loaded in 
    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ProjectileHero goes off-screen destroy it
        if (!bndCheck.LocIs(BoundsCheck.eScreenLocs.onScreen)) Destroy(gameObject);
        
    }
}
