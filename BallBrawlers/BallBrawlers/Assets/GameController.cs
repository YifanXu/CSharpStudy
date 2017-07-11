using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private Dictionary<int, GameObject> enemies;
    public static GameController currentInstance { get; private set; }
    public int NumberOfEnemies;
    public GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
		if(currentInstance != null)
        {
            Destroy(this.gameObject);
        }

        currentInstance = this;
        enemies = new Dictionary<int, GameObject>();

        System.Random r = new System.Random();
        
        for(int i = 0; i < NumberOfEnemies; i++)
        {
            enemies.Add(i, GameObject.Instantiate(enemyPrefab));
            enemies[i].transform.position += new Vector3(r.Next(-20,20),0,r.Next(-20,20));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleDestruction(int id)
    {
        Destroy(enemies[id].gameObject);
        enemies.Remove(id);

        if(enemies.Count == 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
