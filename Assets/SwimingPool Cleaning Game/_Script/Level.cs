using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject[] poolDurts;
    public GameObject Net;
    public ParticleSystem Pickup;
    public ParticleSystem ripple;
    public ParticleSystem bleachPowder;
    public ParticleSystem[] spradeParticle;


    public void shoot(int n)
    {
        if(n==0)
            bleachPowder.Play();

        if (n == 1)
            spradeParticle[0].Play();

        if (n == 2)
            spradeParticle[1].Play();

        if (n == 3)
            spradeParticle[2].Play();
    }


    public void collector(int number)
    {
        float X = Random.Range(-0.4f, 0.9f);
        //float Y = Random.Range(-1.6f, 1.2f);
        float Z = Random.Range(1.5f, -0.7f);
        if (poolDurts[number].transform.tag == "b")
        {

            //poolDurts[number].transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Play();
            Pickup.Play();
            ripple.Play();
            poolDurts[number].transform.parent = Net.transform.GetChild(1).transform;
            poolDurts[number].transform.localPosition = new Vector3(X, 0.7f, Z);
            poolDurts[number].transform.localRotation = Quaternion.Euler(0, 0, 0);
            poolDurts[number].transform.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (poolDurts[number].transform.tag == "a")
        {
            poolDurts[number].transform.parent = Net.transform.GetChild(1).transform;
            poolDurts[number].transform.localPosition = Vector3.zero;
            poolDurts[number].transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }




    }
}
