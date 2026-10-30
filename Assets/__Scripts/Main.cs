using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S; // A private singleton for Main

    [Header("Inscribed")]
    public GameObject[] prefabEnemies; //Array of Enemy prefabs
    public float enemySpawnPerSecond = 0.5f; // # Enemies spawned/second
    public float enemyInsetDefault = 1.5f; // Inset from the sides
    public float gameRestartDelay = 2; //time delay before game is restarted

    private BoundsCheck bndCheck;

    //Awake is called when the scene is first loaded in
    void Awake() {
        S = this; 
        // Set bndCheck to reference the BoundsCheck component on this GameObject
        bndCheck = GetComponent<BoundsCheck>();
        // Invoke SpawnEnemy() once (in 2 seconds, based on default values)
        //nameof uses spellchecking to ensure reliability
        Invoke(nameof(SpawnEnemy), (1f/enemySpawnPerSecond));
    }

    //method for spawning enemies
    //will alter later from spawning at the top to using random x and y coordinates 
    public void SpawnEnemy() {
        //check if there are already 5+ enemies on-screen using Enemy tag
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= 5) {
            //skip spawning but schedule next check/spawn attempt
            Invoke(nameof(SpawnEnemy), (1f/enemySpawnPerSecond)); 
            return;
        }

        // Pick a random Enemy prefab to instantiate
        //choose a random # between 1 and 1-prefabEnemies.Length
        int ndx = Random.Range(0, prefabEnemies.Length); 
        //ndx is used to randomly pick a GameObject prefab from prefabEnemies
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]); 

        // Position the Enemy above the screen with a random x position
        float enemyInset = enemyInsetDefault; //enemyInsetDefault is set in the inspector
        //if the selected enemy prefab has a BoundsCheck, use the radius value from it for enemyInset
        if(go.GetComponent<BoundsCheck>() != null) {
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        //Set the initial position for spawned Enemy
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        // Invoke SpawnEnemy() again
        //oh god why did they make this recursive
        //Invoke() is used instead of InvokeRepeating() so that timing can be changed dynamically for testing
        Invoke(nameof(SpawnEnemy), (1f/enemySpawnPerSecond)); 
    }

    //invokes the Restart() method in gameRestartDelay seconds
    void DelayedRestart() {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    //reloads __Scene_0 to restart the game 
    //will probably retool into game over screen trigger 
    void Restart() {
        SceneManager.LoadScene("__Scene_0");
    }

    //calls DelayedRestart when player dies
    static public void HERO_DIED() {
        S.DelayedRestart();
    }
}
