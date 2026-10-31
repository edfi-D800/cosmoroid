using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance {get; private set;}
    
    private int score = 0; 
    private Text uiText; //reference to text component of this object

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explodeClip; //this will handle enemy explosion sounds

    //Awake is called when the scene is first loaded in
    private void Awake()
    {
        //enforce single instance
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        uiText = GetComponent<Text>(); //find text component and assign the reference to uiText
    }

    //method for adding points to score
    public void AddScore(int points) {
        score += points;
        PlayExplosionSound();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = score.ToString("#,0"); //format text to have commas as per human # rules
        //not to say that i am not human somehow no i love breathing oxygen
    }

    //sfx method for explosion
    public void PlayExplosionSound() {
        if ((audioSource != null) && (explodeClip != null)) {
            audioSource.PlayOneShot(explodeClip);
        }
    }
}
