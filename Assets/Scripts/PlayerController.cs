using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    //public float moveSpeed;
    //Rigidbody rigidBody;
    Camera mainCamera;
    //Vector3 velocity;

    public GameObject currentGridPoint;
    public GenerateGridPoints gridGenerator;

    public bool move;
    Vector3 endPoss;

    public static Action action_playerMoved;
    public static bool playerMoved;

    private float timeSinceLastInput;
    private float playerInputDelay;

    /*  Touch Control    */
    public const float MAX_SWIPE_TIME = 0.5f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    public const float MIN_SWIPE_DISTANCE = 0.05f;

    public static bool swipedRight = false;
    public static bool swipedLeft = false;
    public static bool swipedUp = false;
    public static bool swipedDown = false;

    public bool debugWithArrowKeys = false;

    Vector2 startPos;
    float startTime;
    /*      */

    public Animator playerAnimator;

    public float playerMovementSpeed;

    public GameObject prefab_hit, prefab_hitText;

    public Transform hitImpactPos;

    public GameObject smoke_prefab;

    public LevelManager levelManager;

    public AudioSource audioSource, scream, cry;
    public AudioClip walkingAudio, laughingAudio, scaredAudio;

    public GameObject prefab_emoji;

    public GameObject slapPos;

    void Start()
    {
        debugWithArrowKeys = true;
        playerMovementSpeed = 30f;
        playerInputDelay = 0.5f;
        //rigidBody = GetComponent<Rigidbody>();
        //mainCamera = Camera.main;
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GenerateGridPoints>();

        StartCoroutine(PlayerInput(0.2f));

        timeSinceLastInput = 0;

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void PlayHitImpact(Vector3 hitPos)
    {
        StartCoroutine(DelayText(hitPos));
    }

    IEnumerator DelayText(Vector3 hitPos)
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(prefab_hit, hitPos, Quaternion.identity);
     //   yield return new WaitForSeconds(0.2f);
        Instantiate(prefab_hitText, hitPos, Quaternion.identity);
    }

    public void PlaySlapImpact()
    {
        Instantiate(prefab_hit, slapPos.transform.position, Quaternion.identity);
    }

    IEnumerator PlayerInput(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
        }
    }
    void Update()
    {
        if (currentGridPoint != null && !move && !GameManager.instance.IS_GAMEOVER && !GameManager.instance.PLAYER_CAUGHT)
        {
            if (PlayerPrefs.GetInt("isTutorialDone", 0) == 0)
            {
                PlayerPrefs.SetInt("isTutorialDone", 1);
            }


            if (swipedUp)
            {
                if (currentGridPoint.GetComponent<GridPoint>().isUp && Mathf.Abs(timeSinceLastInput - Time.timeSinceLevelLoad) > playerInputDelay)
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    playerAnimator.SetBool("walking", true);
                    move = true;

                    audioSource.clip = walkingAudio;
                    audioSource.Play();
                    endPoss = new Vector3(transform.position.x, transform.position.y, transform.position.z + currentGridPoint.GetComponent<GridPoint>().U_dist);

                    timeSinceLastInput = Time.timeSinceLevelLoad;
                }
            }
            else if (swipedDown)
            {
                levelManager.tutorialObj.SetActive(false);

                if (currentGridPoint.GetComponent<GridPoint>().isDown && Mathf.Abs(timeSinceLastInput - Time.timeSinceLevelLoad) > playerInputDelay)
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0); move = true;
                    playerAnimator.SetBool("walking", true);
                    move = true;

                    audioSource.clip = walkingAudio;
                    audioSource.Play();

                    endPoss = new Vector3(transform.position.x, transform.position.y, transform.position.z - currentGridPoint.GetComponent<GridPoint>().D_dist);
                    timeSinceLastInput = Time.timeSinceLevelLoad;
                }
            }
            else if (swipedLeft)
            {
                if (currentGridPoint.GetComponent<GridPoint>().isLeft && Mathf.Abs(timeSinceLastInput - Time.timeSinceLevelLoad) > playerInputDelay)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    playerAnimator.SetBool("walking", true);
                    move = true;

                    audioSource.clip = walkingAudio;
                    audioSource.Play();

                    endPoss = new Vector3(transform.position.x - currentGridPoint.GetComponent<GridPoint>().L_dist, transform.position.y, transform.position.z);
                    timeSinceLastInput = Time.timeSinceLevelLoad;
                }
            }
            else if (swipedRight)
            {
                if (currentGridPoint.GetComponent<GridPoint>().isRight && Mathf.Abs(timeSinceLastInput - Time.timeSinceLevelLoad) > playerInputDelay)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    playerAnimator.SetBool("walking", true);
                    move = true;

                    audioSource.clip = walkingAudio;
                    audioSource.Play();

                    endPoss = new Vector3(transform.position.x + currentGridPoint.GetComponent<GridPoint>().R_dist, transform.position.y, transform.position.z);
                    timeSinceLastInput = Time.timeSinceLevelLoad;


                }
            }

            swipedRight = false;
            swipedLeft = false;
            swipedUp = false;
            swipedDown = false;

            if (Input.touches.Length > 0 && !EventSystem.current.IsPointerOverGameObject(0) && !move)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    startPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                    startTime = Time.time;
                }
                if (t.phase == TouchPhase.Moved && !swipedRight && !swipedLeft && !swipedDown && !swipedUp)
                {
                    Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                    Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                    if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                        return;

                    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                    { // Horizontal swipe
                        if (swipe.x > 0)
                        {
                            swipedRight = true;
                            Debug.Log("Swipe right "+gameObject.name);
                        }
                        else
                        {
                            swipedLeft = true;
                            Debug.Log("Swipe left " + gameObject.name );

                        }
                        return;

                    }
                    else
                    { // Vertical swipe
                        if (swipe.y > 0)
                        {
                            swipedUp = true;
                            Debug.Log("Swipe up " + gameObject.name);
                        }
                        else
                        {
                            swipedDown = true;
                            Debug.Log("Swipe down " + gameObject.name);
                        }
                        return;

                    }
                }
                //if (t.phase == TouchPhase.Ended)
                //{
                //    if (Time.time - startTime > MAX_SWIPE_TIME) // press too long
                //        return;

                //    Vector2 endPos = new Vector2(t.position.x / (float)Screen.width, t.position.y / (float)Screen.width);
                //    Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                //    if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                //        return;

                //    if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                //    { // Horizontal swipe
                //        if (swipe.x > 0)
                //        {
                //            swipedRight = true;
                //        }
                //        else
                //        {
                //            swipedLeft = true;
                //        }
                //    }
                //    else
                //    { // Vertical swipe
                //        if (swipe.y > 0)
                //        {
                //            swipedUp = true;
                //        }
                //        else
                //        {
                //            swipedDown = true;
                //        }
                //    }
                //}
            }

            if (debugWithArrowKeys)
            {
                swipedDown = swipedDown || Input.GetKeyDown(KeyCode.DownArrow);
                swipedUp = swipedUp || Input.GetKeyDown(KeyCode.UpArrow);
                swipedRight = swipedRight || Input.GetKeyDown(KeyCode.RightArrow);
                swipedLeft = swipedLeft || Input.GetKeyDown(KeyCode.LeftArrow);


            }

        }

        UpdatePos();
    }

    private void UpdatePos()
    {
        if (move && !GameManager.instance.PLAYER_CAUGHT)   // && !GameManager.instance.IS_GAMEOVER
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoss, playerMovementSpeed * Time.deltaTime);
            if (transform.position == endPoss)
            {
                playerAnimator.SetBool("walking", false);
                move = false;

                audioSource.Stop();

                playerMoved = true;
                //                action_playerMoved?.Invoke();
                Invoke("DelayMove", .3f);
            }
        }
    }

    void DelayMove()
    {
        if(!GameManager.instance.PLAYER_CAUGHT)
        {
            action_playerMoved?.Invoke();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "gridPoint")
        {
            currentGridPoint = other.gameObject;
            currentGridPoint.GetComponent<GridPoint>().PlayerEntered();
            playerAnimator.SetBool("kick", false);
            playerAnimator.SetBool("push", false);

        }
        if (other.tag == "readyForKick")
        {
            if (!GameManager.instance.IS_GAMEOVER)
            {
                if (levelManager.enemies_killed < levelManager.enemies_total-1)
                {
                    playerAnimator.SetBool("push", true);
                }
                else
                {
                    playerAnimator.SetBool("kick", true);
                }
                levelManager.enemies_killed++;

                Debug.Log("enemies_killed" + levelManager.enemies_killed);
                Debug.Log("enemies_total " + levelManager.enemies_total);

                Destroy(other.gameObject);
                if (levelManager.enemies_killed == levelManager.enemies_total)
                {
                    audioSource.Stop();

                    levelManager.audioSourceSlowMo.Play();
                    levelManager.DoSlowMotion();

                    Debug.Log("enemies_killed" + levelManager.enemies_killed);
                    Debug.Log("enemies_total " + levelManager.enemies_total);

                    if (levelManager.enemies_total > 1 && levelManager.enemies_killed == levelManager.enemies_total)
                    {
                        levelManager.goSlow2 = true;
                        levelManager.slowDownFactor = 0.01f;
                        levelManager.slowDownLength = 3;
                        levelManager.MoveCamera(gameObject);
                    }
                    else
                    {
                        levelManager.goSlow = true;
                        levelManager.slowDownFactor = 0.05f;
                        levelManager.slowDownLength = 2;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "gridPoint")
        {

            //Smoke Effect
            GameObject smoke = Instantiate(smoke_prefab, new Vector3(currentGridPoint.transform.position.x, currentGridPoint.transform.position.y + 1f, currentGridPoint.transform.position.z), Quaternion.identity);
            Destroy(smoke, 1);

            currentGridPoint.GetComponent<GridPoint>().PlayerExited();
            currentGridPoint = null;

            
        }
    }
    //// Update is called once per frame
    //void Update()
    //{
    //    Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y));
    //    transform.LookAt(mousePos + Vector3.up * transform.position.y);
    //    velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    //}

    //private void FixedUpdate()
    //{
    //    rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    //}

   void DeactivateSmoke()
    {
        smoke_prefab.SetActive(false);
    }
}
