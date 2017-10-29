using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject custodi;
    [SerializeField] GameObject man;
    [SerializeField] Vector3 custodiEnd;
    [SerializeField] Vector3 manEnd;

    private bool waitingForInput;


    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene("Bathroom");
            }
        }

    }
    
    // Use this for initialization
    void Start () {
        waitingForInput = false;
        StartCoroutine("Mission");
	}

    private IEnumerator Mission()
    {
        Vector3 startPos = custodi.transform.position;
        float t = 0;
        float speed = 5;
        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime * speed;
            custodi.transform.position = Vector3.Lerp(startPos, custodiEnd, t);
        }

        yield return new WaitForSeconds(.7f);

        startPos = man.transform.position;
        t = 0;
        speed = 5;
        while (t < 1f)
        {
            yield return null;
            t += Time.deltaTime * speed;
            man.transform.position = Vector3.Lerp(startPos, manEnd, t);
        }
        waitingForInput = true;
    }
}
