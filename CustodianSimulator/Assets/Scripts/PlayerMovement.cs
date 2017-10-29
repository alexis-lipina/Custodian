using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Direction { North = 0, East = 270, South = 180, West = 90 }
public class PlayerMovement : MonoBehaviour
{
    private int turns;
    [SerializeField] Vector3 missionEnd;
    [SerializeField] Vector3 accomplishedEnd;

    [SerializeField] GameObject mission;
    [SerializeField] GameObject accomplished;

    private bool waitingForInput;
    [SerializeField] string levelName;

    //locations of special tiles
    private List<Vector3> dirtTiles;
    private List<Vector3> trashTiles;
    private List<Vector3> wallTiles;
    private List<Vector3> trashCanTiles;
    private List<Vector3> bucketTiles;
    private List<Vector3> trashBagTiles;
    private List<Vector3> mopTiles;
    private List<Vector3> footprintTiles;
    private List<Vector3> waterTiles;

    //spawnable objects
    [SerializeField] GameObject mopPrefab;
    [SerializeField] GameObject trashbagPrefab;
    [SerializeField] GameObject footprintPrefab;
    [SerializeField] GameObject waterPrefab;
    [SerializeField] GameObject waterFootprintPrefab;

    //player states and stats
    private bool hasMop = false;
    private bool mopDeployed = false;
    private bool hasTrashbag = false;
    [SerializeField] int maxTrash;
    private int currentTrashLevel = 0;
    [SerializeField] int feetDirtyTurns;
    private int dirtyTurnsRemaining;
    private Animator animator;
    [SerializeField] int mopLifespan;
    private int mopTilesLeft;
    private bool isMoving;

    //ui
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject uiMopDeployed;
    [SerializeField] GameObject uiWaterMeter;
    [SerializeField] GameObject uiGarbageMeter;
    [SerializeField] Sprite[] mopDeployedSprites;
    [SerializeField] Sprite[] waterMeterSprites;
    [SerializeField] Sprite[] garbageMeterSprites;
    [SerializeField] GameObject deployMop;
    [SerializeField] GameObject rechargeMop;
    [SerializeField] GameObject takeOutTrash;


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
        waterTiles = new List<Vector3>();

        animator = gameObject.GetComponent<Animator>();

        uiMopDeployed.SetActive(false);
        uiWaterMeter.SetActive(false);
        uiGarbageMeter.SetActive(false);

        hasMop = false;
        mopDeployed = false;
        hasTrashbag = false;

        waitingForInput = false;

        turns = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (dirtTiles.Count == 0 && footprintTiles.Count == 0 && trashTiles.Count == 0)
        {
            waitingForInput = true;
            StartCoroutine("Mission");
        }

        if (waitingForInput)
        {
            if (Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene(levelName);
            }
        }

        //gets movement input
        if (!isMoving && !waitingForInput)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (hasMop)
                {
                    mopDeployed = !mopDeployed;
                    animator.SetBool("mopDeployed", mopDeployed);
                    if (mopDeployed)
                    {
                        SuperHero(deployMop);
                    }
                    UpdateUI();
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
        #region movement calculations
        animator.SetBool("walkBool", true);
        transform.localEulerAngles = new Vector3(0, 0, (float)direction);
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(startPos.x + input.x, startPos.y + input.y);
        float moveSpeed = 4;
        //prevents the player from mvoving into restricted squares
        if (trashCanTiles.Contains(endPos) && hasTrashbag)
        {
            hasTrashbag = false;
            currentTrashLevel = 0;
            animator.SetBool("garbageBool", false);
            SuperHero(takeOutTrash);
            UpdateUI();
        }
        if (bucketTiles.Contains(endPos) && hasMop)
        {
            mopTilesLeft = mopLifespan;
            SuperHero(rechargeMop);
            UpdateUI();
        }
        if (wallTiles.Contains(endPos))
        {
            animator.SetBool("walkBool", false);
            isMoving = false;
            yield break;
        }
        if (trashCanTiles.Contains(endPos) || bucketTiles.Contains(endPos))
        {
            animator.SetBool("walkBool", false);
            isMoving = false;
            yield break;
        }
        if (!hasTrashbag || currentTrashLevel == maxTrash)
        {
            if (trashTiles.Contains(endPos))
            {
                animator.SetBool("walkBool", false);
                isMoving = false;
                yield break;
            }
        }
        //smooth lerp between startPos and endPos
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        #endregion


        //pick up trash if have trash bag and space in bac
        if (hasTrashbag && trashTiles.Contains(transform.position) && currentTrashLevel < maxTrash)
        {
            RemoveTrash();
        }
        //makes footprints in water
        if (waterTiles.Contains(transform.position))
        {
            GameObject water = (from GameObject waterTile in GameObject.FindGameObjectsWithTag("Water") where waterTile.transform.position == transform.position select waterTile).ToList()[0];
            waterTiles.Remove(water.transform.position);
            Destroy(water);
            Instantiate(waterFootprintPrefab, transform.position, transform.rotation);
            footprintTiles.Add(transform.position);
        }
        //pick up trashbag
        if (trashBagTiles.Contains(transform.position) && !hasTrashbag)
        {
            GetTrashbag();
        }
        //pick up mop
        else if (mopTiles.Contains(transform.position) && !hasMop)
        {
            GetMop();
        }
        //gets feet dirty
        if (dirtTiles.Contains(transform.position) && (!mopDeployed || mopTilesLeft == 0))
        {
            dirtyTurnsRemaining = feetDirtyTurns;
        }
        //makes foot prints if feet are dirty
        if (!dirtTiles.Contains(transform.position) && !trashTiles.Contains(transform.position) && dirtyTurnsRemaining > 0)
        {
            dirtyTurnsRemaining--;
            if (dirtyTurnsRemaining < 0) { dirtyTurnsRemaining = 0; }
            footprintTiles.Add(transform.position);
            Instantiate(footprintPrefab, transform.position, transform.rotation);
        }
        //makes water if mop is deployed and have water elft
        if (mopDeployed && mopTilesLeft > 0 && !waterTiles.Contains(startPos))
        {
            //clean dirt if mop deployed and have water left
            if (hasMop && mopDeployed && dirtTiles.Contains(startPos))
            {
                RemoveDirt(startPos);
            }
            //remove footprints if mop deployed and have water left
            if (hasMop && mopDeployed && footprintTiles.Contains(startPos))
            {
                List<GameObject> footprints = (from GameObject footprintTile in GameObject.FindGameObjectsWithTag("Footprint") where footprintTile.transform.position == startPos select footprintTile).ToList();

                for (int i = 0; i < footprints.Count; i++)
                {
                    footprintTiles.Remove(footprints[i].transform.position);
                    Destroy(footprints[i]);
                }
            }
            MopFloor(startPos);
            mopTilesLeft--;


            
        }

        animator.SetBool("walkBool", false);

        UpdateUI();

        isMoving = false;

        turns++;
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
    private void RemoveDirt(Vector3 dirtPos)
    {
        GameObject dirt = (from GameObject dirtTile in GameObject.FindGameObjectsWithTag("Dirt") where dirtTile.transform.position == dirtPos select dirtTile).ToList()[0];
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
            GameObject dropMop = Instantiate(mopPrefab, transform.position, Quaternion.identity);
            Mop mopScript = dropMop.GetComponent<Mop>();
            mopScript.SetWater(mopTilesLeft);
            mopTilesLeft = 0;

            hasMop = false;
            mopDeployed = false;
            animator.SetBool("mopBool", false);
            animator.SetBool("mopDeployed", false);
        }
        hasTrashbag = true;
        animator.SetBool("garbageBool", true);
    }

    /// <summary>
    /// Picks up a mop and drops the trashbag if equiped
    /// </summary>
    private void GetMop()
    {
        GameObject mop = (from GameObject mopTile in GameObject.FindGameObjectsWithTag("Mop") where mopTile.transform.position == transform.position select mopTile).ToList()[0];
        mopTiles.Remove(mop.transform.position);
        mopTilesLeft = mop.GetComponent<Mop>().WaterLeft;
        Destroy(mop);
        if (hasTrashbag)
        {
            trashBagTiles.Add(transform.position);
            GameObject trashbag = Instantiate(trashbagPrefab, transform.position, Quaternion.identity);
            Trashbag trashbagScript = trashbag.GetComponent<Trashbag>();
            trashbagScript.SetTrash(currentTrashLevel);
            currentTrashLevel = 0;

            hasTrashbag = false;
            animator.SetBool("garbageBool", false);
        }
        hasMop = true;
        mopDeployed = false;
        animator.SetBool("mopBool", true);
    }
    private void MopFloor(Vector3 lastPos)
    {
        Instantiate(waterPrefab, lastPos, Quaternion.identity);
        waterTiles.Add(lastPos);
    }


    private void UpdateUI()
    {
        if (hasMop)
        {
            uiGarbageMeter.SetActive(false);
            uiMopDeployed.SetActive(true);
            uiWaterMeter.SetActive(true);
            if (mopDeployed)
            {
                uiMopDeployed.GetComponent<Image>().sprite = mopDeployedSprites[1];
            }
            else
            {
                uiMopDeployed.GetComponent<Image>().sprite = mopDeployedSprites[0];
            }
            uiWaterMeter.GetComponent<Image>().sprite = waterMeterSprites[mopTilesLeft];
        }
        else if (hasTrashbag)
        {
            uiMopDeployed.SetActive(false);
            uiWaterMeter.SetActive(false);
            uiGarbageMeter.SetActive(true);
            uiGarbageMeter.GetComponent<Image>().sprite = garbageMeterSprites[currentTrashLevel];
        }
        else
        {
            uiMopDeployed.SetActive(false);
            uiWaterMeter.SetActive(false);
            uiGarbageMeter.SetActive(false);
        }
    }

    private void SuperHero(GameObject imageToUse)
    {
        Instantiate(imageToUse, transform.position, Quaternion.identity);
    }

    private IEnumerator Mission()
    {
        Vector3 startPos = mission.transform.position;
        float t = 0;
        float speed = 5;
        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime * speed;
            mission.transform.position = Vector3.Lerp(startPos, missionEnd, t);
        }

        startPos = accomplished.transform.position;
        t = 0;
        speed = 5;
        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime * speed;
            accomplished.transform.position = Vector3.Lerp(startPos, accomplishedEnd, t);
        }
    }
}
