using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private List<Vector3> moveRange;

    private static List<Vector3> dirtTiles;
    private static List<Vector3> trashTiles;
    private static List<Vector3> wallTiles;

    /// <summary>
    /// Gets a list of tiles with dirt in them
    /// </summary>
    public static List<Vector3> DirtList
    {
        get { return dirtTiles; }
    }
    /// <summary>
    /// Gets a list of tiles with trash in them
    /// </summary>
    public static List<Vector3> TrashList
    {
        get { return trashTiles; }
    }
    // Use this for initialization
    void Start()
    {
        dirtTiles = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") select dirtTile.transform.position).ToList();
        trashTiles = (from GameObject trashTile in GameObject.FindGameObjectsWithTag("Trash") select trashTile.transform.position).ToList();
        wallTiles = (from GameObject wallTile in GameObject.FindGameObjectsWithTag("Wall") select wallTile.transform.position).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        #region movement input
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                input.y = 1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.S))
            {
                input.y = -1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.D))
            {
                input.x = 1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.A))
            {
                input.x = -1;
                StartCoroutine(Move(input));
            }
        }
        #endregion
    }

    private IEnumerator Move(Vector2 input)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float t = 0;
        Vector3 endPos = new Vector3(startPos.x + input.x, startPos.y + input.y);

        float moveSpeed = 5;

        if (PlayerCleaning.HasMop)
        {
            if(trashTiles.Contains(endPos) || wallTiles.Contains(endPos))
            {
                t = 1f;
            }
        }
        if (PlayerCleaning.HasTrashbag)
        {
            if(dirtTiles.Contains(endPos) || wallTiles.Contains(endPos))
            {
                t = 1f;
            }
        }

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
    }

    /// <summary>
    /// Adds a tile to the dirt list
    /// </summary>
    /// <param name="dirtTile">The position to add</param>
    public static void AddDirt(Vector3 dirtTile)
    {
        dirtTiles.Add(dirtTile);
    }

    /// <summary>
    /// Adds a tile to the trash list
    /// </summary>
    /// <param name="trashTile">The position to add</param>
    public static void AddTrash(Vector3 trashTile)
    {
        trashTiles.Add(trashTile);
    }
    /// <summary>
    /// Removes a tile from the dirt list
    /// </summary>
    /// <param name="dirtTile">The position to remove</param>
    public static void RemoveDirt(Vector3 dirtTile)
    {
        dirtTiles.Remove(dirtTile);
    }
    /// <summary>
    /// Removes a tile from the trash list
    /// </summary>
    /// <param name="trashTile">The position to remove</param>
    public static void RemoveTrash(Vector3 trashTile)
    {
        trashTiles.Remove(trashTile);
    }
}
