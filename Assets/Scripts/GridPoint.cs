using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    public bool isRight, isLeft, isUp, isDown;
    public bool isPlayer;

    public int R_dist, L_dist, U_dist, D_dist;

    public GameObject smoke_prefab;

    public Color og_color, glow_color;

    void Awake()
    {
        // isRight = true;
        // isLeft = true;
        // isUp = true;
        // isDown = true;
    }

    public void PlayerEntered()
    {
        transform.Find("directions").Find("circle").gameObject.SetActive(true);
        if (isUp)
        {
            transform.Find("directions").Find("up").gameObject.SetActive(true);
        }
        if (isDown)
        {
            transform.Find("directions").Find("down").gameObject.SetActive(true);
        }
        if (isLeft)
        {
            transform.Find("directions").Find("left").gameObject.SetActive(true);
        }
        if (isRight)
        {
            transform.Find("directions").Find("right").gameObject.SetActive(true);
        }
    }

    public void PlayerExited()
    {
        transform.Find("directions").Find("circle").gameObject.SetActive(false);
        transform.Find("directions").Find("up").gameObject.SetActive(false);
        transform.Find("directions").Find("down").gameObject.SetActive(false);
        transform.Find("directions").Find("left").gameObject.SetActive(false);
        transform.Find("directions").Find("right").gameObject.SetActive(false);
    }

    void Glow()
    {

    }
}
