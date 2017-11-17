using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighScoreBoard : MonoBehaviour
{
    List<int> highScores;



	// Use this for initialization
	void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);

        highScores = new List<int>();
	}


    public void AddScore(int score)
    {
        if (!highScores.Contains(score))
        {
            highScores.Add(score);
        }
    }


    public int HighestAScore()
    {
        int lowest = int.MaxValue;
        foreach(int tempScore in highScores)
        {
            if(tempScore < lowest)
            {
                lowest = tempScore;
            }
        }

        return lowest;
    }
}
