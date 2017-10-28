using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    private bool isMoving;
    private List<Vector3> moveRange;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        #region movement input
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.W))
            {
                input.y = 1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.S))
            {
                input.y = -1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.D))
            {
                input.x = 1;
                StartCoroutine(Move(input));
            }
        }
        if (!isMoving)
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.A))
            {
                input.x = -1;
                StartCoroutine(Move(input));
            }
        }
        #endregion
    }

    private IEnumerator Move(Vector2 input)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float t = 0;
        Vector3 endPos = new Vector3(startPos.x + input.x, startPos.y + input.y);

        float moveSpeed = 5;

        if (GameController.WallList.Contains(endPos))
        {
            t = 1f;
        }

        if (PlayerCleaning.HasMop)
        {
            if(GameController.TrashList.Contains(endPos))
            {
                t = 1f;
            }
        }
        if (PlayerCleaning.HasTrashbag)
        {
            if(GameController.DirtList.Contains(endPos))
            {
                t = 1f;
            }
        }

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
    }

}
