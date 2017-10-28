using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pow : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("KillThyself");
	}
	
	private IEnumerator KillThyself()
    {
        float killspeed = 2.5f;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * killspeed;
            yield return null;
        }
        Destroy(gameObject);
    }
}
