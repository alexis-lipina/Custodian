using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Direction { North = 0, East = 90, South = 180, West = 270 }
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
    private List<Vector3> footprintTiles;

    //spawnable objects
    [SerializeField] GameObject mopPrefab;
    [SerializeField] GameObject trashbagPrefab;
    [SerializeField] GameObject footprintPrefab;
    
    //player states and stats
    private bool hasMop = false;
    private bool mopDeployed = false;
    private bool hasTrashbag = false;
    [SerializeField] int maxTrash;
    private int currentTrashLevel = 0;
    [SerializeField] int feetDirtyTurns;
    private int dirtyTurnsRemaining;

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
        footprintTiles = new List<Vector3>();

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
            if (Input.GetButtonDown("Submit"))
            {
                if (hasMop)
                {
                    mopDeployed = !mopDeployed;
                }
            }

            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                input.x = 0;
                input.y = 1;
                StartCoroutine(Move(input, Direction.North));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                input.x = 0;
                input.y = -1;
                StartCoroutine(Move(input, Direction.South));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                input.y = 0;
                input.x = 1;
                StartCoroutine(Move(input, Direction.East));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                input.y = 0;
                input.x = -1;
                StartCoroutine(Move(input, Direction.West));
            }
        }
    }
    
    /// <summary>
    /// Moves the player based on input
    /// </summary>
    /// <param name="input">The location to move to</param>
    private IEnumerator Move(Vector2 input, Direction direction)
    {
        isMoving = true;
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x + input.x, startPos.y + input.y);

        //Vector3 startRot = transform.localEulerAngles;

        //float degrees;

        //float a = (float)direction - startRot.z;
        //float b = (float)direction - startRot.z;
        
        //if(System.Math.Min(System.Math.Abs(a), System.Math.Abs(b)) == a)
        //{
        //    degrees = a;
        //}
        //else
        //{
        //    degrees = b;
        //}

        //Vector3 endRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.rotation.z + degrees);


        float moveSpeed = 5;

        //prevents the player from mvoving into restricted squares
        if(trashCanTiles.Contains(endPos) && hasTrashbag)
        {
            hasTrashbag = false;
            currentTrashLevel = 0;
        }
        if (wallTiles.Contains(endPos) || trashCanTiles.Contains(endPos) || bucketTiles.Contains(endPos))
        {
            t = 1f;
        }
        if (!hasTrashbag || currentTrashLevel == maxTrash)
        {
            if(trashTiles.Contains(endPos))
            {
                t = 1f;
            }
        }

        //smooth lerp between startPos and endPos
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            //transform.localEulerAngles = Vector3.Lerp(startRot, endRot, t);

            
            
            yield return null;
        }


        //checks if player ended turn in special tile and acts accordingly
        if(hasTrashbag && trashTiles.Contains(transform.position) && currentTrashLevel < maxTrash)
        {
            RemoveTrash();
        }
        if(hasMop && mopDeployed && dirtTiles.Contains(transform.position))
        {
            RemoveDirt();
        }
        if(hasMop && mopDeployed && footprintTiles.Contains(transform.position))
        {
            List<GameObject> footprints = (from GameObject footprintTile in GameObject.FindGameObjectsWithTag("Footprint") where footprintTile.transform.position == transform.position select footprintTile).ToList();

            for(int i = 0; i < footprints.Count; i++)
            {
                footprintTiles.Remove(footprints[i].transform.position);
                Destroy(footprints[i]);
            }
        }

        if (trashBagTiles.Contains(transform.position) && !hasTrashbag)
        {
            GetTrashbag();
        }
        else if (mopTiles.Contains(transform.position) && !hasMop)
        {
            GetMop();
        }

        if(dirtTiles.Contains(transform.position) && !mopDeployed)
        {
            dirtyTurnsRemaining = feetDirtyTurns;
        }
        if(!dirtTiles.Contains(transform.position) && !trashTiles.Contains(transform.position) && dirtyTurnsRemaining > 0)
        {
            dirtyTurnsRemaining--;
            if(dirtyTurnsRemaining < 0) { dirtyTurnsRemaining = 0; }
            footprintTiles.Add(transform.position);
            Instantiate(footprintPrefab, transform.position, Quaternion.identity);
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
        currentTrashLevel++;
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
        GameObject trashbag = (from GameObject trashbagTile in GameObject.FindGameObjectsWithTag("Trashbag") where trashbagTile.transform.position == transform.position select trashbagTile).ToList()[0];
        trashBagTiles.Remove(trashbag.transform.position);
        currentTrashLevel = trashbag.GetComponent<Trashbag>().TrashLevel;
        Destroy(trashbag);
        if (hasMop)
        {
            mopTiles.Add(transform.position);
            Instantiate(mopPrefab, transform.position, Quaternion.identity);
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
        GameObject mop = (from GameObject mopTile in GameObject.FindGameObjectsWithTag("Mop") where mopTile.transform.position == transform.position select mopTile).ToList()[0];
        mopTiles.Remove(mop.transform.position);
        Destroy(mop);
        if (hasTrashbag)
        {
            trashBagTiles.Add(transform.position);
            GameObject trashbag = Instantiate(trashbagPrefab, transform.position, Quaternion.identity);
            Trashbag trashbagScript = trashbag.GetComponent<Trashbag>();
            trashbagScript.SetTrash(currentTrashLevel);

            currentTrashLevel = 0;
            hasTrashbag = false;
        }
        hasMop = true;
        mopDeployed = false;
    }
}
