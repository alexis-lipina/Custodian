using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbag : MonoBehaviour
{
    private int trashLevel;

    public int TrashLevel
    {
        get{ return trashLevel; }
    }

    public void SetTrash(int trashLevel)
    {
        this.trashLevel = trashLevel;
    }
}
