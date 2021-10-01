using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rippleMovement : MonoBehaviour
{

    public Transform netPosition;

    void Update()
    {
        transform.localPosition = new Vector3(netPosition.position.x, 12f, netPosition.position.z);
        //transform.localRotation = Quaternion.Euler(transform.rotation.x, netPosition.rotation.y, transform.rotation.z);
    }
}
