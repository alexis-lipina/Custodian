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

    [SerializeField] GameObject wall;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject Sink;
    [SerializeField] GameObject Toilet;
    [SerializeField] GameObject TrashCan;
    [SerializeField] GameObject TrashBag;
    [SerializeField] GameObject Bucket;
    [SerializeField] GameObject Mop;
    [SerializeField] GameObject[] Trash;
    [SerializeField] GameObject[] Dirt;

    // Use this for initialization
    void Awake()
    {
        levelMap = new GameObject[width, height];
        detailMap = new GameObject[width, height];
        switch (level)
        {
            case 1:
                DrawRectangle(wall, 2, 1, 13, 8);
                DrawRectangle(floor, 3, 2, 12, 7);
                DrawRectangle(wall, 5, 1, 5, 4);
                DrawRectangle(wall, 8, 1, 8, 4);
                Draw(detailMap, Toilet, 4, 2);
                Draw(detailMap, Toilet, 7, 2);
                Draw(detailMap, Sink, 5, 7);
                Draw(detailMap, Sink, 7, 7);
                Draw(detailMap, Sink, 9, 7);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 6, 3);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 9, 3);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 4, 5);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 6, 6);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 7, 6);
                Draw(detailMap, Trash[Random.Range(0, Trash.Length)], 9, 6);
                Draw(detailMap, TrashCan, 9, 5);
                Draw(detailMap, TrashBag, 10, 6);
                Draw(detailMap, TrashBag, 12, 2);

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
}