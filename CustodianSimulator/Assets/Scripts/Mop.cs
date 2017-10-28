using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mop : MonoBehaviour
{
    [SerializeField] int lifespan;
    int waterLeft;


    private void Start()
    {
        waterLeft = lifespan;
    }

    public int WaterLeft
    {
        get { return waterLeft; }
    }

    public void SetWater(int waterLeft)
    {
        this.waterLeft = waterLeft;
    }
}
