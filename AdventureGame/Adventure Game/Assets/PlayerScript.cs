using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript player;
    public int SpeedPerSecond;

	// Use this for initialization
	void Start () {
		if(player != null)
        {
            Destroy(player.gameObject);
        }

        player = this;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = new Vector3();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            v.x += SpeedPerSecond*Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            v.x -= SpeedPerSecond * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))  
        {
            v.z -= SpeedPerSecond * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            v.z += SpeedPerSecond * Time.deltaTime;
        }
        this.GetComponent<Rigidbody>().AddForce(v);
    }
}
