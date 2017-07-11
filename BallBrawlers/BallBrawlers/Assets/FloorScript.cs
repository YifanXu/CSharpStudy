using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour {
    public int deltaSize;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 vec = new Vector3(deltaSize*Time.deltaTime, 0, deltaSize*Time.deltaTime);
        this.transform.localScale += vec;
	}
}
