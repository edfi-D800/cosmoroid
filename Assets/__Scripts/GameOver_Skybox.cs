using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_Skybox : MonoBehaviour
{
    private Image uiImage;
    public float darkenSpeed = 0.2f; //darkening speed
    private float elapsedTime = 0f;
    private const float duration = 3f; //stopping time in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //grab image
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //check time limit
        if ((elapsedTime < duration) && (uiImage != null)){
            elapsedTime += Time.deltaTime;

            //reduce RGB values on image over time
            Color currentColor = uiImage.color;
            currentColor.r = Mathf.MoveTowards(currentColor.r, 0f, darkenSpeed * Time.deltaTime);
            currentColor.g = Mathf.MoveTowards(currentColor.g, 0f, darkenSpeed * Time.deltaTime);
            currentColor.b = Mathf.MoveTowards(currentColor.b, 0f, darkenSpeed * Time.deltaTime);
            
            uiImage.color = currentColor;
        }
    }
}
