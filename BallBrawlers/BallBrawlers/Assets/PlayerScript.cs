using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {
    public int Speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 dir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            dir = new Vector3(0, 0, Speed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir = new Vector3(0, 0, -Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir = new Vector3(Speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir = new Vector3(-Speed * Time.deltaTime, 0, 0);
        }

        this.GetComponent<Rigidbody>().AddForce(dir,ForceMode.Acceleration);

        if(this.transform.position.y < -10)
        {
            SceneManager.LoadScene("FailScene");
        }
	}
}
