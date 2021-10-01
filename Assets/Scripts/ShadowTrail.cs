using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTrail : MonoBehaviour
{
    public float timeBetweenSpawns;
    public float startTimeBetweenSpawns;
    public GameObject prefab;

    void Update()
    {
        if (gameObject.GetComponent<PlayerController>().move)
        {
            if (timeBetweenSpawns <= 0)
            {
                GameObject obj = Instantiate(prefab, transform.position, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z));
                timeBetweenSpawns = startTimeBetweenSpawns;
                Destroy(obj, 0.2f);
            }
            else
            {
                timeBetweenSpawns -= Time.deltaTime;
            }
        }
    }
}
