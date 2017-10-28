using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    [SerializeField] private int wallToUse;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetFloat("Blend", wallToUse);
    }
}
