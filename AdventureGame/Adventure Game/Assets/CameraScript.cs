using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = new Vector3(PlayerScript.player.transform.position.x - this.transform.position.x, 0, PlayerScript.player.transform.position.z - this.transform.position.z);
        this.transform.position += v;
    }
}
