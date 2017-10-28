using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallController : MonoBehaviour {

    private int wallToUse = 0;

    private List<Vector3> floorTiles;
    private Animator anim;

    void Start()
    {
        floorTiles = (from GameObject floorTile in GameObject.FindGameObjectsWithTag("Floor") select floorTile.transform.position).ToList();
        anim = GetComponent<Animator>();

        bool[,] neighbors = new bool[3, 3];
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                neighbors[i, j] = floorTiles.Contains(new Vector3(transform.position.x+(j-1), transform.position.y+(i-1), transform.position.z));
            }
        }

        // seriously bad code ahead
        if (neighbors[1,0] && neighbors[0,1] && neighbors[1,2])
        {
            wallToUse = 14;
        }
        else if (neighbors[0,1] && neighbors[1,2] && neighbors[2,1])
        {
            wallToUse = 13;
        }

        else if (neighbors[1,2] && neighbors[2,1] && neighbors[1,0])
        {
            wallToUse = 12;
        }
        else if (neighbors[2,1] && neighbors[1,0] && neighbors[0,1])
        {
            wallToUse = 15;
        }
        else if (neighbors[1,0] && neighbors[1,2])
        {
            wallToUse = 17;
        }
        else if (neighbors[0,1] && neighbors[2,1])
        {
            wallToUse = 16;
        }
        else if (neighbors[2,1] && neighbors[1,2])
        {
            wallToUse = 8;
        }
        else if (neighbors[1,0] && neighbors[2,1])
        {
            wallToUse = 9;
        }
        else if (neighbors[1,2] && neighbors[0,1])
        {
            wallToUse = 10;
        }
        else if (neighbors[0,1] && neighbors[1,0])
        {
            wallToUse = 11;
        }
        else if (neighbors[2,1])
        {
            wallToUse = 0;
        }
        else if (neighbors[1,2])
        {
            wallToUse = 1;
        }
        else if (neighbors[0,1])
        {
            wallToUse = 2;
        }
        else if (neighbors[1,0])
        {
            wallToUse = 3;
        }
        else if (neighbors[2,0] && neighbors[2,2])
        {
            wallToUse = 21;
        }
        else if (neighbors[2,2] && neighbors[0,2])
        {
            wallToUse = 20;
        }
        else if (neighbors[0,0] && neighbors[0,2])
        {
            wallToUse = 18;
        }
        else if (neighbors[0,0] && neighbors[2,0])
        {
            wallToUse = 19;
        }
        else if (neighbors[2,2])
        {
            wallToUse = 4;
        }
        else if (neighbors[2,0])
        {
            wallToUse = 5;
        }
        else if (neighbors[0,2])
        {
            wallToUse = 6;
        }
        else if (neighbors[0,0])
        {
            wallToUse = 7;
        }

        anim.SetFloat("Blend", wallToUse);
    }
}
