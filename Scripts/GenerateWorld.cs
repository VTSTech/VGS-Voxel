using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

public class GenerateWorld : MonoBehaviour
	{

	public int seed = 0;
	void Awake ()
	{
		seed = (int)Network.time * 20;	
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	public int GetSeed ()
	{
        return seed;
	}
}
