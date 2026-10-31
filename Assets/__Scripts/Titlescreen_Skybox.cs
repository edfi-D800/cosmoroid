using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Titlescreen_Skybox : MonoBehaviour
{
    private Image uiImage;
    public float startDelay = 1f; //time delay before scene switchover 

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explodeClip; //this will handle a funny sound

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //grab image
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //detect keystroke
        if (Input.anyKeyDown) {
            DelayedFart();
        }

    }

    //these are called fart because start is called automatically
    void Fart() {
        SceneManager.LoadScene("__Scene_0");
    }

    void DelayedFart() {
        //play cool sound
        PlayExplosionSound();
        //ATTEMPT to invert ui image color, of course it doesn't work fully... 
        if (uiImage != null) {
            Color currentColor = uiImage.color;
            uiImage.color = new Color(1f - currentColor.r, 1f - currentColor.g, 1f - currentColor.b, currentColor.a);
        }
        Invoke(nameof(Fart), startDelay);
    }

    //sfx method for explosion
    public void PlayExplosionSound() {
        if ((audioSource != null) && (explodeClip != null)) {
            audioSource.PlayOneShot(explodeClip);
        }
    }
}
