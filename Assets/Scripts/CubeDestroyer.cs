using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    void Start()
    {
        Invoke("Delay", 3);
    }

    void Delay()
    {
        Destroy(gameObject);
    }
}
