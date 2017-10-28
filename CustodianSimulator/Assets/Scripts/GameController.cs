using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    #region static lists
    private static List<Vector3> dirtTiles;
    private static List<Vector3> trashTiles;
    private static List<Vector3> wallTiles;
    private static List<Vector3> trashCanTiles;
    private static List<Vector3> bucketTiles;
    private static List<Vector3> trashBagTiles;
    private static List<Vector3> mopTiles;

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
    #endregion


    // Use this for initialization
    void Start ()
    {
        dirtTiles = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") select dirtTile.transform.position).ToList();
        trashTiles = (from GameObject trashTile in GameObject.FindGameObjectsWithTag("Trash") select trashTile.transform.position).ToList();
        wallTiles = (from GameObject wallTile in GameObject.FindGameObjectsWithTag("Wall") select wallTile.transform.position).ToList();
        trashCanTiles = (from GameObject trashCanTile in GameObject.FindGameObjectsWithTag("TrashCan") select trashCanTile.transform.position).ToList();
        bucketTiles = (from GameObject bucketTile in GameObject.FindGameObjectsWithTag("Bucket") select bucketTile.transform.position).ToList();
        trashBagTiles = (from GameObject trashBagTile in GameObject.FindGameObjectsWithTag("TrashBag") select trashBagTile.transform.position).ToList();
        mopTiles = (from GameObject mopTile in GameObject.FindGameObjectsWithTag("Mop") select mopTile.transform.position).ToList();

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
    /// Adds a tile to the trash can list
    /// </summary>
    /// <param name="trashCanTile">The position to add</param>
    public static void AddTrashCan(Vector3 trashCanTile)
    {
        trashCanTiles.Add(trashCanTile);
    }
    /// <summary>
    /// Adds a bucket to the bucket list
    /// </summary>
    /// <param name="bucketTile">The position to add</param>
    public static void AddBucket(Vector3 bucketTile)
    {
        bucketTiles.Add(bucketTile);
    }
    /// <summary>
    /// Adds a trash back to the trash bag list
    /// </summary>
    /// <param name="trashBagTile">The position to add</param>
    public static void AddTrashBag(Vector3 trashBagTile)
    {
        trashBagTiles.Add(trashBagTile);
    }
    /// <summary>
    /// Adds a mop to the mop list
    /// </summary>
    /// <param name="mopTile">The position to add</param>
    public static void AddMop(Vector3 mopTile)
    {
        mopTiles.Add(mopTile);
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
