using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Text levelText;
    public Text scoreText;
    public Button buttonCarrot;
    public Button buttonForward;
    public Button buttonPlay;

    int level;

	// Use this for initialization
	void Start () {
        level = 1;
        levelText.text = "" + level;
        scoreText.text = "0/0";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space))
        {
            levelText.text = "" + level++;
        }
	}
}
