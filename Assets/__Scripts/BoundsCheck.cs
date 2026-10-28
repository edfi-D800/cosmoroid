using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a GameObject on-screen.
/// Note that this ONLY works for an orthographic Main Camera.
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Dynamic")]
    public float camWidth;
    public float camHeight;

    //Awake is called when the scene is first loaded in 
    void Awake() {
        //set camHeight to MainCamera's orthographicSize value
        camHeight = Camera.main.orthographicSize;
        //set camWidth to camHeight and aspect as defined by the current aspect ratio
        //this should mean camWidth is set to the distance to the left or right screen edge
        camWidth = camHeight * Camera.main.aspect;
    }

    //LateUpdate is called every frame after Update() is called on all GameObjects
    void LateUpdate()
    {
        Vector3 pos = transform.position;

        //if camWidth is exceeded, reset to other side
        if (pos.x > camWidth) {
            pos.x = -camWidth;
        }
        if (pos.x < -camWidth) {
            pos.x = camWidth;
        }

        //if camHeight is exceeded, reset to other side
        if (pos.y > camHeight) {
            pos.y = -camHeight;
        }
        if (pos.y < -camHeight) {
            pos.y = camHeight;
        }

        transform.position = pos;
    }

}
