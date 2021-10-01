using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateGridPoints : MonoBehaviour
{
    public int yPos;
    float startPosx, startPosz;
    public int gridSize;
    public float offset;

    public GameObject pf_gridPoint, plane;
    public GameObject pf_player;

    void Start()
    {
        startPosx = (-gridSize / 2) * offset + (offset / 2);
        startPosz = (-gridSize / 2) * offset + (offset / 2);
     //   Generate();

     //   SpawnPlayer();
    }

    void Generate()
    {
      //  plane.transform.position = new Vector3(plane.transform.position.x - offset / 2, yPos, plane.transform.position.z - offset / 2);

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject obj = Instantiate(pf_gridPoint, new Vector3(startPosx + (i * offset), yPos, startPosz + (j * offset)), Quaternion.identity);
                obj.transform.localScale = new Vector3(.2f, .2f, .2f);
                obj.transform.parent = transform;

                if (j == 0)
                {
                    obj.GetComponent<GridPoint>().isDown = false;
                }else 
                if (j == (gridSize - 1))
                {
                    obj.GetComponent<GridPoint>().isUp = false;
                }

                if (i == 0)
                {
                    obj.GetComponent<GridPoint>().isLeft = false;
                }
                else
                if (i == (gridSize - 1))
                {
                    obj.GetComponent<GridPoint>().isRight = false;
                }
            }
        }
    }

    void SpawnPlayer()
    {
        Instantiate(pf_player, new Vector3(startPosx, yPos, startPosz), Quaternion.identity);
    }
}
