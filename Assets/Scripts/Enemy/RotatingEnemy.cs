using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemy : MonoBehaviour
{
    public bool rotateRight;
    public int rotatingAngle;
    private bool isRotate;

    public float targetRotation;
    public float moveSpeed;
    public GameObject img_moveDir;

    private void Awake()
    {
        moveSpeed = 30f;
        PlayerController.action_playerMoved += RotateEnemy;

        if (!rotateRight)
        {
            img_moveDir.transform.localScale = new Vector3(-img_moveDir.transform.localScale.x, img_moveDir.transform.localScale.y, img_moveDir.transform.localScale.z);
        }
        
    }

    private void OnDestroy()
    {
        PlayerController.action_playerMoved -= RotateEnemy;
    }

    void RotateEnemy()
    {
        if (!GameManager.instance.IS_GAMEOVER)
        {
//            Invoke("DelayRotate", 0.3f);
            DelayRotate();
        }
    }

    void DelayRotate()
    {
        isRotate = true;

        if (rotateRight)
        {
            targetRotation = transform.eulerAngles.y + rotatingAngle;
            //    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + rotatingAngle, transform.eulerAngles.z);
            //    StartCoroutine(RotateRight(transform.eulerAngles.y + rotatingAngle));
        }
        else
        {
            targetRotation = transform.eulerAngles.y - rotatingAngle;
            //   transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - rotatingAngle, transform.eulerAngles.z);
            //    StartCoroutine(RotateLeft(transform.eulerAngles.y - rotatingAngle));
        }
    }

    //IEnumerator RotateLeft(float targetRotation)
    //{
    //    Debug.Log("targetRotation "+ targetRotation);
    //    float moveSpeed = 1f;
    //    while (transform.rotation.y < targetRotation)
    //    {
    //        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), moveSpeed * Time.time);
    //        Debug.Log("rotating");
    //        yield return null;
    //    }
    //    transform.rotation = Quaternion.Euler(0, targetRotation, 0);

    //    Debug.Log("rotate left enddddddddddddd");
    //    yield return null;
    //}

    //IEnumerator RotateRight(float targetRotation)
    //{
    //    Debug.Log("targetRotation "+ targetRotation);
    //    float moveSpeed = 0.1f;
    //    while (transform.rotation.y < targetRotation)
    //    {
    //        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), moveSpeed * Time.time);
    //        Debug.Log("rotating");
    //        yield return null;
    //    }
    // //   transform.rotation = Quaternion.Euler(0, targetRotation, 0);

    //    Debug.Log("rotate right enddddddddddddd");
    //    yield return null;
    //}

    private void Update()
    {
        if (!gameObject.GetComponent<Enemy>().isDead && isRotate)
        {
            if (rotateRight)
            {
                if (transform.eulerAngles.y < targetRotation)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), moveSpeed * Time.deltaTime);
                }
                else
                {
                    isRotate = false;
                }
            }
            else
            {
                if (transform.eulerAngles.y > targetRotation)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), moveSpeed * Time.deltaTime);
                }
                else
                {
                    isRotate = false;
                }
            }
        }
    }
}
