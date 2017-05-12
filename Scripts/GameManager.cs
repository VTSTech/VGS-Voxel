using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

/* 
 * v0.0.1-r04
 * Written by Veritas83
 * www.NigelTodman.com
 * /Scripts/GameManager.cs
 */

public class GameManager : MonoBehaviour {
public static GameManager Instance
{
	get
  {
  	return instance;
  }
}
private static GameManager instance = null;
public bool IsPaused = false;
public string SetPlayerName = "Player";
public bool isGameOver = false;
public int seed = 0;
void Awake()
  {
		seed = (int)Network.time * 20;	
    if(instance)
    {
        DestroyImmediate(gameObject);
        return;
    }
    instance = this;
    DontDestroyOnLoad(gameObject);
}

// Use this for initialization
void Start () {
	
}

// Update is called once per frame
void Update () {
	
}
public int GetSeed ()
{
      return seed;
}
}
