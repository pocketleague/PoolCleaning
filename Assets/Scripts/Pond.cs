using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pond : MonoBehaviour
{
    public GameObject waterSplash_pf;
    public GameObject prefab_emoji;

    public LevelManager levelManager;
    public AudioSource audioSourceSplash;
    public AudioSource audioSourceOther;


    public AudioClip laughingClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "forSplash")
        {
            Debug.Log("fell at position "+other.transform.position);
            Instantiate(waterSplash_pf, other.transform.position, Quaternion.identity);

            audioSourceSplash.Play();

            audioSourceOther.clip = laughingClip;
            audioSourceOther.Play();

            // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = other.transform.position;
            // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = new Vector3(other.transform.position.x, other.transform.position.y - 3.5f, other.transform.position.z);

            levelManager.content.transform.GetChild(levelManager.enemies_killed - 1).GetChild(0).gameObject.SetActive(true);

            StartCoroutine(DelayForFloat(other.gameObject));
        }else if (other.tag == "forSplashPlayer")
        {
            Debug.Log("fell at position " + other.transform.position);
            Instantiate(waterSplash_pf, other.transform.position, Quaternion.identity);

            audioSourceSplash.Play();

            audioSourceOther.clip = laughingClip;
            audioSourceOther.Play();

            // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = other.transform.position;
            // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = new Vector3(other.transform.position.x, other.transform.position.y - 3.5f, other.transform.position.z);

         //   levelManager.content.transform.GetChild(levelManager.enemies_killed - 1).GetChild(0).gameObject.SetActive(true);

            StartCoroutine(DelayForFloatPlayer(other.gameObject));
        }
    }

    IEnumerator DelayForFloat(GameObject other)
    {

        yield return new WaitForSeconds(0.3f);

        //Water Splash on the screen camera
        if (levelManager.enemies_killed == levelManager.enemies_total)
        {
            Color c = new Color32(255, 255, 255, 10);
            //   CameraPlay.BloodHit(c, 2f, 0.5f);
            CameraPlay.RainDrop_ON(2);
        }


        other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
       // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = other.transform.position;
     //   other.transform.parent.GetComponent<EnemyFloat>().initialPosition = new Vector3(other.transform.position.x, other.transform.position.y - 1.5f, other.transform.position.z);

        yield return new WaitForSeconds(1);
    //    other.transform.parent.parent = null;
        other.transform.parent.Find("Enemy").GetComponent<Animator>().SetBool("floating", true);

        //  yield return new WaitForSeconds(1);

        if (levelManager.enemies_killed != levelManager.enemies_total)
        {
            GameObject emoji = other.transform.parent.GetComponent<Enemy>().emoji_canvas;
            emoji.SetActive(true);
            emoji.GetComponentInChildren<Animator>().SetInteger("angry", Random.Range(1, 4));
            Debug.Log("llllllllll");
            //GameObject emoji = Instantiate(prefab_emoji, other.transform.parent.Find("imogi_pos").position, Quaternion.Euler(-51.64f, 180, 0), other.transform.parent);
            //emoji.GetComponent<Animator>().SetInteger("angry", Random.Range(1, 5));
            //yield return new WaitForSeconds(3);
            //Destroy(emoji);
        }

        yield return new WaitForSeconds(1);

        if (levelManager.enemies_killed == levelManager.enemies_total)
        {
        //    CameraPlay.RainDrop_OFF(2);
        }

        //   other.transform.parent.GetComponent<EnemyFloat>().isFloating = true;
    }

    IEnumerator DelayForFloatPlayer(GameObject other)
    {
        Debug.Log("float start");
        yield return new WaitForSeconds(0.4f);
        other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        // other.transform.parent.GetComponent<EnemyFloat>().initialPosition = other.transform.position;
        //   other.transform.parent.GetComponent<EnemyFloat>().initialPosition = new Vector3(other.transform.position.x, other.transform.position.y - 1.5f, other.transform.position.z);

        yield return new WaitForSeconds(.5f);
        //    other.transform.parent.parent = null;
        other.transform.parent.Find("Player").GetComponent<Animator>().SetBool("floating", true);

        yield return new WaitForSeconds(.5f);

//        other.transform.parent.GetComponent<PlayerController>().cry.Play();
        //  yield return new WaitForSeconds(1);

        //if (levelManager.enemies_killed != levelManager.enemies_total)
        //{
        //    GameObject emoji = Instantiate(prefab_imogi, other.transform.parent.Find("imogi_pos").position, Quaternion.Euler(-51.64f, 180, 0), other.transform.parent);
        //    emoji.GetComponent<Animator>().SetInteger("angry", Random.Range(1, 5));
        //    yield return new WaitForSeconds(3);
        //    Destroy(emoji);
        //}
        //   other.transform.parent.GetComponent<EnemyFloat>().isFloating = true;
    }
}
