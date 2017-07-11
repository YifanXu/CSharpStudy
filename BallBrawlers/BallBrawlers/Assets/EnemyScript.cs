using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public float walkInterval;
    public int Speed;
    public int ID;
    private static int IDCount = 0;
    private static System.Random r;
    private float TimeLeft;
    private Vector3 currentVector;

	// Use this for initialization
	void Start () {
        r = new System.Random();
        ID = IDCount;
        IDCount++;
        TimeLeft = walkInterval;
        currentVector = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {

        /*        int xDif = (int)this.transform.position.x;
                int zDif = (int)this.transform.position.z;
                double scale = Speed *Time.deltaTime / Mathf.Sqrt((int)(xDif * xDif + zDif * zDif));
                Vector3 vec = new Vector3((int) (this.transform.position.x * scale * -1), 0, (int) (this.transform.position.z * scale * -1));
                this.transform.position += vec;*/
        if (TimeLeft <= 0)
        {
            int roll = r.Next(4);
            switch (roll)
            {
                case 0:
                    currentVector = new Vector3(Speed * Time.deltaTime, 0, 0);
                    break;
                case 1:
                    currentVector = new Vector3(Speed * Time.deltaTime * -1, 0, 0);
                    break;
                case 2:
                    currentVector = new Vector3(0, 0, Speed * Time.deltaTime);
                    break;
                case 3:
                    currentVector = new Vector3(0, 0, Speed * Time.deltaTime * -1);
                    break;
            }
            TimeLeft = walkInterval;
        }
        else
        {
            TimeLeft -= Time.deltaTime;
        }
        this.transform.position += currentVector;

        if (this.transform.position.y < -10)
        {
            GameController.currentInstance.HandleDestruction(this.ID);
        }
	}
}
