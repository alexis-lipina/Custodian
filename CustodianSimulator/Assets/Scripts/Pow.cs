using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pow : MonoBehaviour
{
    System.Random rng;
	// Use this for initialization
	void Start ()
    {
        rng = new System.Random();
        transform.localScale = new Vector3(.1f, .1f);
        StartCoroutine("KillThyself");
	}
	
	private IEnumerator KillThyself()
    {
        Vector3 startPos = transform.position;
        Vector3 finalDestination = new Vector3(rng.Next(40, 200), rng.Next(40, 200));
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        float killspeed = .5f;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * killspeed;
            //transform.position = Vector3.Lerp(startPos, finalDestination, t);
            Vector3 big = new Vector3(.2f, .2f);
            transform.localScale += big;

            yield return null;
        }
        Destroy(gameObject);
    }
}
