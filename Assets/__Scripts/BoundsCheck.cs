using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a GameObject on-screen.
/// Note that this ONLY works for an orthographic Main Camera.
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    //allows control over wether BoundsCheck is centered, inset, or outset through the inspector
    public enum eType {center, inset, outset};

    [System.Flags]
    public enum eScreenLocs {
        onScreen = 0, // 0000 in binary (zero)
        offRight = 1, // 0001 in binary (zero)
        offLeft = 2, // 0010 in binary (zero)
        offUp = 4, // 0100 in binary (zero)
        offDown = 8, // 1000 in binary (zero)
    }

    [Header("Inscribed")]
    public eType boundsType = eType.center;
    public float radius = 1f;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public bool keepOnScreen = true;
    //width and height for game camera
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
        //Assign the checkRadius that will enable center, inset, or outset options
        float checkRadius = 0;
        if (boundsType == eType.inset) checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;

        //if camWidth is exceeded, reset to other side
        if (pos.x > (camWidth + checkRadius)) {
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }
        if (pos.x < (-camWidth - checkRadius)) {
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }

        //if camHeight is exceeded, reset to other side
        if (pos.y > (camHeight + checkRadius)) {
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }
        if (pos.y < (-camHeight - checkRadius)) {
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }

        //only update pos id keepOnScreen and !isOnScreen is true
        //honestly not sure if i need this in the first place for my purposes
        if (keepOnScreen && !isOnScreen) {
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
        }
    }

    //backwards compatibility for removed isOnScreen field
    public bool isOnScreen {
        get {return (screenLocs == eScreenLocs.onScreen);}
    }
}
