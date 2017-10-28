using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private GameObject player;

    void LateUpdate()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.z = transform.position.z;
        transform.position = newPosition;
    }
}
