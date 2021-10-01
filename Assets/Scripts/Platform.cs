using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    Vector3 startPos = new Vector3();

    public Material mat1, mat2;
    private bool isColorChanged;

    private bool shake;

    public GameObject bounceTrigger;
    float sinwaveVal;
    public GameObject smoke_prefab;

    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        startPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            // Spin object around Y-Axis
            //   transform.parent.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;

            sinwaveVal = Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
            if (sinwaveVal < 0)
            {
                sinwaveVal *= -1;
            }
            tempPos.y -= sinwaveVal;

            transform.parent.position = tempPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ppppppp" + other.name);

        if (other.tag == "player" && !isColorChanged)
        {
            Debug.Log("gggggggg" + other.name);

            //   isColorChanged = true;

            if (other.gameObject.GetComponent<PlayerController>().move)
            {
                gameObject.GetComponent<MeshRenderer>().material = mat2;

                shake = true;

                Debug.Log("qqqqqqq");

                bounceTrigger.SetActive(true);
                Invoke("DisableShake", .3f);

                GameObject smoke = Instantiate(smoke_prefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                Destroy(smoke, 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //   gameObject.GetComponent<MeshRenderer>().material = mat1;
        if (other.tag == "player" && !isColorChanged)
        {
            StartCoroutine(DisableTrail());

            DisableShake();
        }

    }

    IEnumerator DisableTrail()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<MeshRenderer>().material = mat1;
    }

    void DisableShake()
    {
        shake = false;
        transform.parent.position = startPos;
        bounceTrigger.SetActive(false);
    }
}
