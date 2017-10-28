using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCleaning : MonoBehaviour {

    private bool hasMop = false;
    private bool hasTrashbag = false;
    private int numOfTrash = 0;
    private List<Vector3> dirtList;
    private List<Vector3> trashList;

    public bool HasMop {
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
    public bool HasTrashbag {
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

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}
}
