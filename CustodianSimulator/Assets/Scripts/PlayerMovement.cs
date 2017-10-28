using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    //locations of special tiles
    private List<Vector3> dirtTiles;
    private List<Vector3> trashTiles;
    private List<Vector3> wallTiles;
    private List<Vector3> trashCanTiles;
    private List<Vector3> bucketTiles;
    private List<Vector3> trashBagTiles;
    private List<Vector3> mopTiles;

    //spawnable objects
    [SerializeField] GameObject mop;
    [SerializeField] GameObject trashbag;
    
    //player states
    private bool hasMop = false;
    private bool mopDeployed = false;
    private bool hasTrashbag = false;

    //control fields
    private bool isMoving;


    void Start()
    {
        //gets all special tiles
        dirtTiles = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") select dirtTile.transform.position).ToList();
        trashTiles = (from GameObject trashTile in GameObject.FindGameObjectsWithTag("Trash") select trashTile.transform.position).ToList();
        wallTiles = (from GameObject wallTile in GameObject.FindGameObjectsWithTag("Wall") select wallTile.transform.position).ToList();
        trashCanTiles = (from GameObject trashCanTile in GameObject.FindGameObjectsWithTag("Trashcan") select trashCanTile.transform.position).ToList();
        bucketTiles = (from GameObject bucketTile in GameObject.FindGameObjectsWithTag("Bucket") select bucketTile.transform.position).ToList();
        trashBagTiles = (from GameObject trashBagTile in GameObject.FindGameObjectsWithTag("Trashbag") select trashBagTile.transform.position).ToList();
        mopTiles = (from GameObject mopTile in GameObject.FindGameObjectsWithTag("Mop") select mopTile.transform.position).ToList();

        hasMop = false;
        mopDeployed = false;
        hasTrashbag = false;
    }

    // Update is called once per frame
    void Update()
    {

        //gets movement input
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                input.x = 0;
                input.y = 1;
                StartCoroutine(Move(input));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                input.x = 0;
                input.y = -1;
                StartCoroutine(Move(input));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                input.y = 0;
                input.x = 1;
                StartCoroutine(Move(input));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                input.y = 0;
                input.x = -1;
                StartCoroutine(Move(input));
            }
        }
    }
    
    /// <summary>
    /// Moves the player based on input
    /// </summary>
    /// <param name="input">The location to move to</param>
    private IEnumerator Move(Vector2 input)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float t = 0;
        Vector3 endPos = new Vector3(startPos.x + input.x, startPos.y + input.y);
        float moveSpeed = 5;

        //prevents the player from mvoving into restricted squares
        if (wallTiles.Contains(endPos) || trashCanTiles.Contains(endPos) || bucketTiles.Contains(endPos))
        {
            t = 1f;
        }
        if (!hasTrashbag)
        {
            if(trashTiles.Contains(endPos))
            {
                t = 1f;
            }
        }
        //if (hasTrashbag)
        //{
        //    if (dirtTiles.Contains(endPos))
        //    {
        //        t = 1f;
        //    }
        //}

        //smooth lerp between startPos and endPos
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }


        //checks if player ended turn in special tile and acts accordingly
        if(hasTrashbag && trashTiles.Contains(transform.position))
        {
            RemoveTrash();
        }
        if(hasMop && mopDeployed && dirtTiles.Contains(transform.position))
        {
            RemoveDirt();
        }

        if (trashBagTiles.Contains(transform.position))
        {
            GetTrashbag();
        }
        else if (mopTiles.Contains(transform.position))
        {
            GetMop();
        }
        isMoving = false;
    }

    /// <summary>
    /// Removes trash from the scene
    /// </summary>
    private void RemoveTrash()
    {
        GameObject trash = (from GameObject trashTile in GameObject.FindGameObjectsWithTag("Trash") where trashTile.transform.position == transform.position select trashTile).ToList()[0];
        trashTiles.Remove(trash.transform.position);
        Destroy(trash);
    }
    
    /// <summary>
    /// Removes dirt from the scene
    /// </summary>
    private void RemoveDirt()
    {
        GameObject dirt = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") where dirtTile.transform.position == transform.position select dirtTile).ToList()[0];
        dirtTiles.Remove(dirt.transform.position);
        Destroy(dirt);
    }

    /// <summary>
    /// Picks up a trash bag and drops the mop if equiped
    /// </summary>
    private void GetTrashbag()
    {
        GameObject trashbag = (from GameObject trashbagTile in trashBagTiles where trashbagTile.transform.position == transform.position select trashbagTile).ToList()[0];
        trashBagTiles.Remove(trashbag.transform.position);
        Destroy(trashbag);
        if (hasMop)
        {
            mopTiles.Add(transform.position);
            Instantiate(mop, transform.position, Quaternion.identity);
            hasMop = false;
            mopDeployed = false;
        }
        hasTrashbag = true;
    }

    /// <summary>
    /// Picks up a mop and drops the trashbag if equiped
    /// </summary>
    private void GetMop()
    {
        GameObject mop = (from GameObject mopTile in mopTiles where mopTile.transform.position == transform.position select mopTile).ToList()[0];
        mopTiles.Remove(mop.transform.position);
        Destroy(mop);
        if (hasTrashbag)
        {
            trashBagTiles.Add(transform.position);
            Instantiate(trashbag, transform.position, Quaternion.identity);
            hasTrashbag = false;
        }
        hasMop = true;
        mopDeployed = false;
    }
}
