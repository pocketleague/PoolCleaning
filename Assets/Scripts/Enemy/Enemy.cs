using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [Range(0, 360)]
    public float viewAngle;
    public float viewRadius;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    Color old_fovColor;


    public bool playerCaught, playerPushed, startPlayerHit;
    public bool isDead;

    // Explosion
    public float cubeSize = 0.2f;
    public int cubesInRow = 5;

    float cubesPivotDistance;
    Vector3 cubesPivot;

    public float explosionForce = 50f;
    public float explosionRadius = 4f;
    public float explosionUpward = 0.4f;

    public GameObject currentGridPoint;

    public Action EnemyReachedGridPoint;

    public Animator animator;

    public int enemyFallFprce;

    public AudioSource audioSource, audioSourceWalk;
    public AudioClip [] audioClip;
    public AudioClip audioClip_slap;

    private float enemyCatchSpeed;

    public GameObject emoji_canvas, shadow;

    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        enemyCatchSpeed = 0.2f;

        StartCoroutine(FindTargetWithDelay(0.01f));
    }

    private void Update()
    {
        if (startPlayerHit && !GameManager.instance.IS_GAMEOVER)
        {
            Debug.Log("Catching player");

            //            Debug.Log("currentGridPoint "+ currentGridPoint);

            //    if ((visibleTargets[0].position.z == transform.position.z) && (visibleTargets[0].position.x - transform.position.x) > 0 && currentGridPoint.GetComponent<GridPoint>().isRight) // Target is in +X axis 
            if (currentGridPoint.GetComponent<GridPoint>().isRight && transform.forward == new Vector3(1.0f, 0.0f, 0.0f)) // Target is in +X axis 
            {
                Debug.Log("Target is in Right ");
                transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, enemyCatchSpeed);
            }
            //else if ((visibleTargets[0].position.z == transform.position.z) && (visibleTargets[0].position.x - transform.position.x) < 0 && currentGridPoint.GetComponent<GridPoint>().isLeft) // Target is in -X axis 
            else if (currentGridPoint.GetComponent<GridPoint>().isLeft && transform.forward == new Vector3(-1.0f, 0.0f, 0.0f)) // Target is in -X axis 
            {
                Debug.Log("Target is in Left ");
                transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, enemyCatchSpeed);
            }
            //else if (transform.forward == new Vector3(0.0f, 0.0f, 1.0f) && currentGridPoint.GetComponent<GridPoint>().isUp) // Target is in +Z axis 
            else if (currentGridPoint.GetComponent<GridPoint>().isUp && (transform.forward == new Vector3(0.0f, 0.0f, 1.0f))) // Target is in +Z axis 
            {
                Debug.Log("Target is Up ");
                transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, enemyCatchSpeed);
            }
            //else if ((visibleTargets[0].position.x == transform.position.x) && (visibleTargets[0].position.z - transform.position.z) > 0 && currentGridPoint.GetComponent<GridPoint>().isUp) // Target is in +Z axis 
            //{
            //    Debug.Log("Target is Up ");
            //    transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, enemyCatchSpeed);
            //}
            //else if ((visibleTargets[0].position.x == transform.position.x) && (visibleTargets[0].position.z - transform.position.z) < 0 && currentGridPoint.GetComponent<GridPoint>().isDown) // Target is in -Z axis 
            else if (currentGridPoint.GetComponent<GridPoint>().isDown && transform.forward == new Vector3(0.0f, 0.0f, -1.0f)) // Target is in -Z axis 
            {
                Debug.Log("Target is Down ");
                transform.position = Vector3.MoveTowards(transform.position, visibleTargets[0].position, enemyCatchSpeed);
            }
            else
            {
                startPlayerHit = false;
                playerCaught = false;
                GameManager.instance.PLAYER_CAUGHT = false;
                GameManager.instance.comingFromFront = false;
            }

        }
    }

    bool CheckCanKill()
    {
        Debug.Log("Checking for kill ");

     //   if ((visibleTargets[0].position.z == transform.position.z) && (visibleTargets[0].position.x - transform.position.x) > 0 && currentGridPoint.GetComponent<GridPoint>().isRight) // Target is in +X axis 
        if (currentGridPoint.GetComponent<GridPoint>().isRight && transform.forward == new Vector3(1.0f, 0.0f, 0.0f)) // Target is in +X axis 
        {
            Debug.Log("Target is in Right ");
            return true;
        }
//        else if ((visibleTargets[0].position.z == transform.position.z) && (visibleTargets[0].position.x - transform.position.x) < 0 && currentGridPoint.GetComponent<GridPoint>().isLeft) // Target is in -X axis 
        else if (currentGridPoint.GetComponent<GridPoint>().isLeft && transform.forward == new Vector3(-1.0f, 0.0f, 0.0f)) // Target is in -X axis 
        {
            Debug.Log("Target is in Left ");
            return true;
        }
        //(Mathf.Abs(visibleTargets[0].position.x - transform.position.x) > 1)  && (visibleTargets[0].position.z - transform.position.z) > 0 && 
        else if (currentGridPoint.GetComponent<GridPoint>().isUp && (transform.forward == new Vector3(0.0f, 0.0f, 1.0f))) // Target is in +Z axis 
        {
            Debug.Log("Target is Up ");
            return true;
        }
        //else if ((visibleTargets[0].position.x == transform.position.x) && (visibleTargets[0].position.z - transform.position.z) > 0 && currentGridPoint.GetComponent<GridPoint>().isUp) // Target is in +Z axis 
        //{
        //    Debug.Log("Target is Up ");
        //    return true;
        //}
       // else if ((visibleTargets[0].position.x == transform.position.x) && (visibleTargets[0].position.z - transform.position.z) < 0 && currentGridPoint.GetComponent<GridPoint>().isDown) // Target is in -Z axis 
        else if (currentGridPoint.GetComponent<GridPoint>().isDown && transform.forward == new Vector3(0.0f, 0.0f, -1.0f)) // Target is in -Z axis 
        {
            Debug.Log("Target is Down ");
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDead)
        {
            if (other.tag == "gridPoint")
            {
                currentGridPoint = other.gameObject;
                EnemyReachedGridPoint?.Invoke();
            }
            else
            if (other.tag == "player")
            {
                if (playerCaught)
                {
                    GameManager.instance.IS_GAMEOVER = true;
                //    other.gameObject.GetComponent<Animator>().SetBool("death", true);
                    GameManager.instance.PlayerDied();
                    Debug.Log("game over");
                }
                else
                {
                    float headingAngle = Quaternion.LookRotation(other.transform.forward).eulerAngles.y;
                    headingAngle = Quaternion.LookRotation(transform.forward).eulerAngles.y;

                    // Kill Enemy
                    Debug.Log("Kill enemy");
                    Destroy(gameObject.transform.Find("FieldOfView").gameObject);

                    if (gameObject.name == "EnemyRotating")
                    {
                        Destroy(gameObject.transform.Find("moveDir").gameObject);
                    }

                    isDead = true;
                    //    explode();
                    other.GetComponent<PlayerController>().PlayHitImpact(transform.Find("hitTextpos").position);

                    StartCoroutine(Fall(other.transform));
                    GameManager.instance.KillEnemy(other.transform);
                }
            }
        }
        
    }
    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            //    FindVisibleTarget();
            if (!isDead) // && PlayerController.playerMoved
            {
                DetectPlayer();
            }
        }
    }
    void FindVisibleTarget()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void DetectPlayer()
    {
        if (!playerCaught)
        {
            visibleTargets.Clear();
            playerPushed = false;

            Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetInViewRadius.Length; i++)
            {
                Transform target = targetInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        if (!playerPushed)
                        {
                            visibleTargets.Add(target);
                            playerPushed = true;

                            if (CheckCanKill())
                            {
                                playerCaught = true;
                                GameManager.instance.PLAYER_CAUGHT = true;

                                if (target.transform.forward == transform.forward * -1)
                                {
                                    GameManager.instance.comingFromFront = true;
                                }

                             //   target.GetComponent<PlayerController>().move = false;
                                target.GetComponent<PlayerController>().audioSource.Stop();

                                target.Find("Player").GetComponent<Animator>().SetBool("halt", true);

                                old_fovColor = viewMeshFilter.GetComponent<MeshRenderer>().material.color;
                                viewMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);


                                StartCoroutine(DelayCatch(target));
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator DelayCatch(Transform target)
    {
       
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);

        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);

        GameObject emoji = target.GetComponent<PlayerController>().prefab_emoji;
        emoji.SetActive(true);
        emoji.GetComponentInChildren<Animator>().SetBool("scared", true);

        //GameObject emoji = Instantiate(target.GetComponent<PlayerController>().prefab_emoji, target.transform.Find("imogi_pos").position, Quaternion.Euler(-51.64f, 180, 0), target.transform);
        //emoji.GetComponent<Animator>().SetBool("scared", true);

        target.GetComponent<PlayerController>().scream.Play();

        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);

        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);

        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(0.05f);
        viewMeshFilter.GetComponent<MeshRenderer>().enabled = true;

       
        viewMeshFilter.GetComponent<MeshRenderer>().material.SetColor("_Color", old_fovColor);

        //  yield return new WaitForSeconds(0.3f);
        

        yield return new WaitForSeconds(0.5f);

        startPlayerHit = true;
        audioSourceWalk.Play();

        animator.SetBool("catch", true);

        yield return new WaitForSeconds(0.5f);
        audioSourceWalk.Stop();

        //Hit Impact
        target.GetComponent<PlayerController>().PlaySlapImpact();
        audioSource.clip = audioClip_slap;
        audioSource.Play();


        target.Find("Player").GetComponent<Animator>().SetBool("death", true);
        target.GetComponent<Rigidbody>().AddForce(transform.right * -enemyFallFprce);
        target.GetComponent<Rigidbody>().useGravity = true;
        //        Destroy(emoji);
        emoji.SetActive(false);
        // yield return new WaitForSeconds(1);

        yield return new WaitForSeconds(1);
        GameManager.instance.IS_GAMEOVER = true;
        //    other.gameObject.GetComponent<Animator>().SetBool("death", true);
        GameManager.instance.PlayerDied();
        Debug.Log("game over");

    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool anglesIsGlobal)
    {
        if (!anglesIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount-1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }


    // Explosion Code Ahead
    public void explode()
    {
        //calculate pivot distance
        cubesPivotDistance = cubeSize * cubesInRow / 2;
        //use this value to create pivot vector)
        cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);

        //make object disappear
        gameObject.SetActive(false);

        //loop 3 times to create 5x5x5 pieces in x,y,z coordinates
        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        //get explosion position
        Vector3 explosionPos = transform.position;
        //get colliders in that position and radius
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        //add explosion force to all colliders in that overlap sphere
        foreach (Collider hit in colliders)
        {
            //get rigidbody from collider object
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    void createPiece(int x, int y, int z)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - cubesPivot;
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;

        Destroy(piece, 3);
    }
    // Explosion Code End

    IEnumerator Fall(Transform playerTransform)
    {
        yield return new WaitForSeconds(0.1f);

     //   Debug.Log("Player looking at "+ transform.forward);

        audioSource.clip = audioClip[UnityEngine.Random.Range(0, 3)];
        audioSource.Play();

        shadow.SetActive(false);

        if (playerTransform.forward == new Vector3(-1.0f, 0.0f, 0.0f))
        {
        //    Debug.Log("kicking from down");
           
            if (transform.forward == new Vector3(-1.0f, 0.0f, 0.0f))  // Looking left
            {
         //       Debug.Log("enemy falling right");
                animator.SetBool("fallRight", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * enemyFallFprce);
            }
            else if (transform.forward == new Vector3(1.0f, 0.0f, 0.0f))  // Looking Right
            {
          //      Debug.Log("enemy falling left");
                animator.SetBool("fallLeft", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * -enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, 1.0f))  // Looking Up
            {
          //      Debug.Log("enemy falling front");
                animator.SetBool("fallFront", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.forward * enemyFallFprce);
            }

            if (playerTransform.GetComponent<PlayerController>().levelManager.enemies_killed == playerTransform.GetComponent<PlayerController>().levelManager.enemies_total)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * enemyFallFprce * 4);
                Invoke("AddDownForce", 1);
            }

        }
        else if (playerTransform.forward == new Vector3(1.0f, 0.0f, 0.0f))
        {
//            Debug.Log("kicking from up");
            if (transform.forward == new Vector3(-1.0f, 0.0f, 0.0f))  // Looking left
            {
          //      Debug.Log("enemy falling left");
                animator.SetBool("fallLeft", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * -enemyFallFprce);
            }
            else if (transform.forward == new Vector3(1.0f, 0.0f, 0.0f))  // Looking Right
            {
  //              Debug.Log("enemy falling right");
                animator.SetBool("fallRight", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, -1.0f))  // Looking Down
            {
    //            Debug.Log("enemy falling front");
                animator.SetBool("fallFront", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.forward * enemyFallFprce);
            }

            if (playerTransform.GetComponent<PlayerController>().levelManager.enemies_killed == playerTransform.GetComponent<PlayerController>().levelManager.enemies_total)
            {
                GetComponent<Rigidbody>().AddForce(transform.up * enemyFallFprce * 4);
                Invoke("AddDownForce", 1);
            }

        }
        else if (playerTransform.forward == new Vector3(0f, 0.0f, 1.0f))
        {
      //      Debug.Log("kicking from left");
            
            if (gameObject.transform.forward == new Vector3(1.0f, 0.0f, 0.0f))  // Looking Right
            {
        //        Debug.Log("enemy falling front");
                animator.SetBool("fallFront", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.forward * enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, 1.0f))  // Looking Up
            {
        //        Debug.Log("enemy falling right");
                animator.SetBool("fallRight", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, -1.0f))  // Looking Down
            {
         //       Debug.Log("enemy falling left");
                animator.SetBool("fallLeft", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * -enemyFallFprce);
            }

            //if (playerTransform.GetComponent<PlayerController>().levelManager.enemies_killed == playerTransform.GetComponent<PlayerController>().levelManager.enemies_total)
            //{
            //    GetComponent<Rigidbody>().AddForce(transform.up * enemyFallFprce * 4);
            //    Invoke("AddDownForce", 1);
            //}

        }
        else if (playerTransform.forward == new Vector3(0f, 0.0f, -1.0f))
        {
     //       Debug.Log("kicking from right");

            if (transform.forward == new Vector3(-1.0f, 0.0f, 0.0f))  // Looking Left
            {
    //            Debug.Log("enemy falling front");
                animator.SetBool("fallFront", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.forward * enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, 1.0f))  // Looking Up
            {
      //          Debug.Log("enemy falling left");

                animator.SetBool("fallLeft", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * -enemyFallFprce);
            }
            else if (transform.forward == new Vector3(0.0f, 0.0f, -1.0f))  // Looking Down
            {
      ////          Debug.Log("enemy falling right");
                animator.SetBool("fallRight", true);
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().AddForce(transform.right * enemyFallFprce);
            }

            //if (playerTransform.GetComponent<PlayerController>().levelManager.enemies_killed == playerTransform.GetComponent<PlayerController>().levelManager.enemies_total)
            //{
            //    GetComponent<Rigidbody>().AddForce(transform.up * enemyFallFprce * 4);
            //    Invoke("AddDownForce", 1);
            //}
        }
    }

    void AddDownForce()
    {
        GetComponent<Rigidbody>().AddForce(transform.up * enemyFallFprce * -4);
    }


}
