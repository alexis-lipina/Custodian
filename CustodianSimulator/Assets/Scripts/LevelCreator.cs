using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{

    [SerializeField] private int level;
    private GameObject[,] levelMap;
    private GameObject[,] detailMap;
    private int width = 18;
    private int height = 11;

    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject floorPrefab;
    [SerializeField] GameObject SinkPrefab;
    [SerializeField] GameObject ToiletPrefab;
    [SerializeField] GameObject TrashPrefab;

    // Use this for initialization
    void Start()
    {
        levelMap = new GameObject[width, height];
        //FillTest();
        detailMap = new GameObject[width, height];
        switch (level)
        {
            case 1:
                DrawRectangle(wallPrefab, 2, 1, 13, 8);
                DrawRectangle(floorPrefab, 3, 2, 12, 7);
                DrawRectangle(wallPrefab, 5, 1, 5, 4);
                DrawRectangle(wallPrefab, 8, 1, 8, 4);

                break;
        }
        AssignPositions(levelMap);
        AssignPositions(detailMap);
    }

    //draws a rectangle of floor tiles from start to end position
    public void DrawRectangle(GameObject type, int startCol, int startRow, int endCol, int endRow)
    {
        for (int c = startCol; c <= endCol; c++)
        {
            for (int r = startRow; r <= endRow; r++)
            {
                //NOTE: all detail elements must be added individually
                Draw(levelMap, type, c, r);
            }
        }
    }

    //Adds a gameObject to specific location in array
    public void Draw(GameObject[,] map, GameObject type, int col, int row)
    {
        map[col, row] = type;
    }

    //instantiate all the gameObjects in correct positions
    private void AssignPositions(GameObject[,] map)
    {
        for (int c = 0; c < map.GetLength(0); c++)
        {
            for (int r = 0; r < map.GetLength(1); r++)
            {
                if (map[c, r] != null)
                {
                    Instantiate(map[c, r], new Vector3(c - 6, r - 5, 0), Quaternion.identity);
                }
            }
        }
    }
    /* fills map with floor tiles 
    private void FillTest() {
        for (int c = 0; c < levelMap.GetLength(0); c++)
        {
            for (int r = 0; r < levelMap.GetLength(1); r++)
            {
                levelMap[c, r] = floorPrefab;
                Instantiate(levelMap[c, r], new Vector3(c - 6, r - 5, 0), Quaternion.identity);
            }
        }
    }*/
}