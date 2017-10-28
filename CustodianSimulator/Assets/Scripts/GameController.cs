using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{

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
    /// <summary>
    /// Gets a list of tiles with walls in them
    /// </summary>
    public static List<Vector3> WallList
    {
        get { return wallTiles; }
    }
    // Use this for initialization



    // Use this for initialization
    void Start ()
    {
        dirtTiles = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") select dirtTile.transform.position).ToList();
        trashTiles = (from GameObject trashTile in GameObject.FindGameObjectsWithTag("Trash") select trashTile.transform.position).ToList();
        wallTiles = (from GameObject wallTile in GameObject.FindGameObjectsWithTag("Wall") select wallTile.transform.position).ToList();

    }

    // Update is called once per frame
    void Update () {
		
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
