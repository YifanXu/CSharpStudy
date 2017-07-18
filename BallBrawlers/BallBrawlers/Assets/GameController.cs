using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    private Dictionary<int, GameObject> enemies;
    public static GameController currentInstance { get; private set; }
    public int NumberOfEnemies;
    public GameObject enemyPrefab;
    public int AmountOfParticles;
    public GameObject particlePrefab;

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

        SpawnParticles(0, 5, 0, AmountOfParticles);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleDestruction(int id)
    {
        GameObject obj = enemies[id].gameObject;
        Destroy(obj);
        enemies.Remove(id);

        if(enemies.Count == 0)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    private void SpawnParticles (int x, int y, int z, int amouunt)
    {
        for (int i = 0; i < AmountOfParticles; i++)
        {
            var particle = GameObject.Instantiate(particlePrefab);
            particle.transform.position += new Vector3(x, y, z);
        }
    }
}
