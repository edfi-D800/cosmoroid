using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundsCheck))]

public class ProjectileEnemy : MonoBehaviour
{
    private BoundsCheck bndCheck; //reference to BoundsCheck component
    public GameObject Owner {get; private set;} //stores reference to projectile firer

    public void Initialize(GameObject shooter) {
        Owner = shooter;
    }

    //Awake is called when the scene is first loaded in 
    void Awake() {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ProjectileEnemy goes off-screen destroy it
        if (!bndCheck.LocIs(BoundsCheck.eScreenLocs.onScreen)) Destroy(gameObject);
        
    }
}
