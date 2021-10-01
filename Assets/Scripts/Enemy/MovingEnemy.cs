using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public bool moveHorizontal;
    public bool moveVertical;

    public bool moveLeft, moveUp;
    public bool moveEnemy;

    bool isMoving;
    Vector3 endPoss;
    public GenerateGridPoints gridGenerator;
    //   public GameObject currentGridPoint;

    public Animator enemyAnimator;
    public AudioSource audioSourceWalk;

    private void Awake()
    {
        PlayerController.action_playerMoved += MoveEnemy;
        GetComponent<Enemy>().EnemyReachedGridPoint += EnemyReachedGridPoint;
    }

    private void OnDestroy()
    {

        PlayerController.action_playerMoved -= MoveEnemy;

    }
    void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GenerateGridPoints>();
    }

    void LateUpdate()
    {
        if (moveEnemy && GetComponent<Enemy>().currentGridPoint != null && !gameObject.GetComponent<Enemy>().playerCaught)
        {
            moveEnemy = false;
            if (moveHorizontal)
            {
                if (moveLeft)
                {
                    if (GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isLeft)
                    {
                        isMoving = true;
                        endPoss = new Vector3(transform.position.x - GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().L_dist, transform.position.y, transform.position.z);
                        audioSourceWalk.Play();
                    }
                }
                else
                {
                    if (GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isRight)
                    {
                        isMoving = true;
                        endPoss = new Vector3(transform.position.x + GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().R_dist, transform.position.y, transform.position.z);
                        audioSourceWalk.Play();
                    }
                }
            }
            else
            if (moveVertical)
            {
                if (moveUp)
                {
                    if (GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isUp)
                    {
                        isMoving = true;
                        endPoss = new Vector3(transform.position.x, transform.position.y, transform.position.z + GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().U_dist);
                        audioSourceWalk.Play();
                    }
                }
                else
                {
                    if (GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isDown)
                    {
                        isMoving = true;
                        endPoss = new Vector3(transform.position.x, transform.position.y, transform.position.z - GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().D_dist);
                        audioSourceWalk.Play();
                    }
                }
            }
        }

        UpdatePos();
    }

    private void UpdatePos()
    {
        if (isMoving)   // && !GameManager.instance.IS_GAMEOVER
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoss, .3f);
            if (transform.position == endPoss)
            {
                isMoving = false;
                audioSourceWalk.Stop();
                enemyAnimator.SetBool("walking", false);

            }
        }
    }

    void EnemyReachedGridPoint()
    {
        if (moveHorizontal)       // Moving Horizontal
        {
            if (moveLeft)
            {
                if (!GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isLeft)
                {
                    TurnPlayerAroundHorizontal();
                }
            }
            else
            {
                if (!GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isRight)
                {
                    TurnPlayerAroundHorizontal();
                }
            }
        }
        else if (moveVertical)
        {                   // Moving Vertical
            if (moveUp)
            {
                if (!GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isUp)
                {
                    TurnPlayerAroundVertical();
                }
            }
            else
            {
                if (!GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().isDown)
                {
                    TurnPlayerAroundVertical();
                }
            }
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "gridPoint")
    //    {
    //        currentGridPoint = other.gameObject;

    //        if (moveHorizontal)       // Moving Left
    //        {
    //            if (moveLeft)
    //            {
    //                if (!currentGridPoint.GetComponent<GridPoint>().isLeft)
    //                {
    //                    TurnPlayerAround();
    //                }
    //            }
    //            else
    //            {
    //                if (!currentGridPoint.GetComponent<GridPoint>().isRight)
    //                {
    //                    TurnPlayerAround();
    //                }
    //            }
    //        }
    //        else
    //        {                   // Moving Right
    //            if (currentGridPoint.GetComponent<GridPoint>().isUp)
    //            {
    //                TurnPlayerAround();
    //            }
    //        }
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "gridPoint")
        {
            GetComponent<Enemy>().currentGridPoint.GetComponent<GridPoint>().PlayerExited();
            //    GetComponent<Enemy>().currentGridPoint = null;
        }
    }

    void MoveEnemy()
    {
        if (!GameManager.instance.IS_GAMEOVER)
        {
            Debug.Log("move enemy");
            moveEnemy = true;
            PlayerController.playerMoved = false;
            enemyAnimator.SetBool("walking", true);
        }
    }

    void TurnPlayerAroundHorizontal()
    {
        if (!GameManager.instance.IS_GAMEOVER)
        {
            if (moveLeft)
            {
                moveLeft = false;
            }
            else
            {
                moveLeft = true;
            }
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        }
    }

    void TurnPlayerAroundVertical()
    {
        if (!GameManager.instance.IS_GAMEOVER)
        {
            if (moveUp)
            {
                moveUp = false;
            }
            else
            {
                moveUp = true;
            }
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
        }
    }
}
