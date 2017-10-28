using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCleaning : MonoBehaviour {

    private static bool hasMop = false;
    private static bool hasTrashbag = false;
    private static bool mopDeployed = false;

    private int numOfTrash = 0;
    private List<Vector3> dirtList;
    private List<Vector3> trashList;

    public static bool HasMop {
        get { return hasMop; }
        set
        {
            if (value)
            {
                hasMop = true;
                hasTrashbag = false;
            }
        }
    }
    public static bool HasTrashbag {
        get { return hasTrashbag; }
        set
        {
            if (value)
            {
                hasTrashbag = true;
                hasMop = false;
            }
        }
    }
    public static bool MopDeployed
    {
        get { return mopDeployed; }
        set
        {
            if (hasMop)
            {
                mopDeployed = value;
            }
        }
    }
    public int NumOfTrash { get { return numOfTrash; } }

	// Use this for initialization
	void Start () {
        dirtList = GameController.DirtList;
        trashList = GameController.TrashList;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasMop)
        {
            checkDirt();
        }
        else if (hasTrashbag)
        {
            checkTrash();
        }
	}

    private void checkDirt()
    {
        if(dirtList.Contains(transform.position))
        {
            GameController.RemoveDirt(transform.position);
        }
    }

    private void checkTrash()
    {
        if (trashList.Contains(transform.position))
        {
            GameController.RemoveTrash(transform.position);
        }
    }
}
