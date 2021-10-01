using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public bool IS_GAMEOVER, PLAYER_CAUGHT, comingFromFront;

    public LevelManager levelManager;
    public GameObject gameOverText, btn_replay;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {     
            //If instance already exists and it's not this:
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    public void PlayerDied()
    {
        gameOverText.SetActive(true);
        btn_replay.SetActive(true);
    }

    public void KillEnemy(Transform hit_transform){
        
        levelManager.EnemyKilled(hit_transform.gameObject);
    }
    
    void DelaySpawn()
    {
        levelManager.level++;
        if (levelManager.level >= levelManager.list_levels.Length)
        {
            levelManager.level = 0;
        }

     //   PlayerPrefs.SetInt("level", levelManager.level);

        levelManager.SpawnLevel();
    }
}
