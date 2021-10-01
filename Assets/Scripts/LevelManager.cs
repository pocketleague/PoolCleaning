using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
	public int level;
//    [HideInInspector]
    public int enemies_total, enemies_killed;

	public GameObject [] list_levels;

	private GameObject spawnedLevel;

	public Text txt_level;

    public Camera mainCamera;

    public GameObject gameOver_panel;


    // Camera movement
    private bool moveCamera;
    private Vector3 cameraInititalPos;
    private Quaternion cameraInitialRot;
    private Vector3 cameraEndPos;
    private Quaternion cameraEndRot;
    private float cameraMoveSpeed = 1;

    // Slowmo Effect
    private float freezeTime = 1;
    private float freezeTimer;
    public float slowDownFactor = 0.05f;
    public float slowDownLength = 2f;
    public float slowDownFactor2 = 0.01f;
    public float slowDownLength2 = 10f;

    public bool goSlow, goSlow2;

    public GameObject props;

    public AudioSource audioSourceGameWin;
    public AudioClip gameWinClip;

    public AudioSource audioSourceSlowMo;

    public GameObject content, prefb_dot;

    public GameObject tutorialObj;

    void Start()
    {

        PlayerPrefs.DeleteKey("level");

        if (PlayerPrefs.GetInt("isTutorialDone", 0) == 0)
        {
            tutorialObj.SetActive(true);
        }

        level = PlayerPrefs.GetInt("level", 1);

        SpawnLevel();

        cameraInititalPos = mainCamera.transform.position;
        cameraInitialRot = mainCamera.transform.rotation;

    }

    private void Update()
    {
        if (goSlow)
        {
            //freezeTimer += Time.unscaledDeltaTime;
            //if (freezeTimer > freezeTime)
            //{
            if (cameraMoveSpeed < 10)
            {
                cameraMoveSpeed += Time.unscaledDeltaTime * 2;
            }
            Time.timeScale += (1 / slowDownLength) * Time.unscaledDeltaTime;

                if (Time.timeScale >= 1)
                {
                    goSlow = false;
                    audioSourceSlowMo.Stop();
                }

          //  }
        }else if (goSlow2)
        {
            //freezeTimer += Time.unscaledDeltaTime;
            //if (freezeTimer > freezeTime)
            //{
            if (cameraMoveSpeed < 10)
            {
                cameraMoveSpeed += Time.unscaledDeltaTime * 2;
            }
            Time.timeScale += (1 / slowDownLength) * Time.unscaledDeltaTime;

            if (Time.timeScale >= 1)
            {
                goSlow2 = false;
                audioSourceSlowMo.Stop();
            }

            //  }
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
     //   Time.fixedDeltaTime = Time.timeScale * 0.2f;
    }

    

    void LateUpdate()
    {
        if (moveCamera)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraEndPos, Time.deltaTime * cameraMoveSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(mainCamera.transform.eulerAngles.x, cameraEndRot.eulerAngles.x, Time.deltaTime * cameraMoveSpeed),
                Mathf.LerpAngle(mainCamera.transform.eulerAngles.y, cameraEndRot.eulerAngles.y, Time.deltaTime * cameraMoveSpeed),
                Mathf.LerpAngle(mainCamera.transform.eulerAngles.z, cameraEndRot.eulerAngles.z, Time.deltaTime * cameraMoveSpeed)

                );

            mainCamera.transform.eulerAngles = currentAngle;
        }
    }

    public void SpawnLevel(){
        CameraPlay.RainDrop_OFF(.1f);

        GameManager.instance.PLAYER_CAUGHT = false;
        GameManager.instance.IS_GAMEOVER = false;
    	txt_level.text = "Level "+level.ToString();

    	Destroy(spawnedLevel);
        Debug.Log("level "+level);
    	spawnedLevel = Instantiate(list_levels[level - 1], transform);
        enemies_total = spawnedLevel.GetComponent<LevelData>().total_enemies;
        enemies_killed = 0;

        for (int i = 0; i < content.transform.childCount; i++)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < enemies_total; i++)
        {
            Instantiate(prefb_dot, content.transform);
        }
        mainCamera.orthographicSize = spawnedLevel.GetComponent<LevelData>().cameraSize;
    }

    public void EnemyKilled(GameObject player)
    {
     //    enemies_killed++;

         if (enemies_killed == enemies_total)
         {
            GameManager.instance.IS_GAMEOVER = true;

            level++;

            string eventName = "af_fakeImpression";
            Dictionary<string, string> eventParams = new Dictionary<string, string>() { { "imp", "1" } }; AppsFlyer.trackRichEvent(eventName, eventParams);

            if (level > list_levels.Length)
            {
                level = 1;
            }

            PlayerPrefs.SetInt("level",level);

            player.transform.Find("Player").GetComponent<Animator>().SetBool("dancing"+Random.Range(1,4), true);


            //  player.transform.Find("Player").GetComponent<Animator>().SetBool("laughing", true);

            //    player.transform.Find("Player").GetComponent<Animator>().SetBool("dancing3", true);

            //    Invoke("SpawnLevel", 3);

            Invoke("ShowGameOver", 2);

            if (enemies_total == 1)
            {
                cameraEndPos = player.transform.Find("cameraPos").position;
                cameraEndRot = player.transform.Find("cameraPos").rotation;

             //   Invoke("DelayCamMove", 1);
                StartCoroutine(DelayCamMoveNormal(player));
            }
        }
        else
        {
            player.transform.Find("Player").GetComponent<Animator>().SetBool("laughing", true);
        }
    }

    public void MoveCamera(GameObject player)
    {
        if (player.transform.forward == new Vector3(-1.0f, 0.0f, 0.0f) || player.transform.forward == new Vector3(0.0f, 0.0f, -1.0f))
        {
            cameraEndPos = player.transform.Find("cameraPos3").position;
            cameraEndRot = player.transform.Find("cameraPos3").rotation;
        }
        else
        {
            cameraEndPos = player.transform.Find("cameraPos2").position;
            cameraEndRot = player.transform.Find("cameraPos2").rotation;
        }

        moveCamera = true;

        StartCoroutine(DelayCamMoveSideways(player));
        //props.SetActive(false);
        //moveCamera = true;

        //audioSourceGameWin.clip = gameWinClip;
        //audioSourceGameWin.Play();
        //player.transform.Find("Confetti").gameObject.SetActive(true);
    }

    IEnumerator DelayCamMoveSideways(GameObject player)
    {

        yield return new WaitForSeconds(1);
        props.SetActive(false);
        moveCamera = true;

        //    yield return new WaitForSeconds(1);

        audioSourceGameWin.clip = gameWinClip;
        audioSourceGameWin.Play();

        player.transform.Find("ConfettiSide").gameObject.SetActive(true);
    }

    IEnumerator DelayCamMoveNormal(GameObject player)
    {

        yield return new WaitForSeconds(2);
        props.SetActive(false);
        moveCamera = true;

        //    yield return new WaitForSeconds(1);

        audioSourceGameWin.clip = gameWinClip;
        audioSourceGameWin.Play();

        player.transform.Find("Confetti").gameObject.SetActive(true);

    }

    public void ReplayLevel()
    {
        SpawnLevel();
    }

    void ShowGameOver()
    {
        gameOver_panel.SetActive(true);

    }
    public void NextLevel()
    {
        props.SetActive(true);

        moveCamera = false;
        mainCamera.transform.position = cameraInititalPos;
        mainCamera.transform.rotation = cameraInitialRot;

        SpawnLevel();
    }
}
