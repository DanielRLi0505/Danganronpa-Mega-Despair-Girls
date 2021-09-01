using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    GameObject enemy;
    public string enemyName;
    [SerializeField] GameObject enemyPrefab;
    public bool spawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnEnemy();
    }

    public bool isVisible()
    {

        float camViewHalfHeight = Camera.main.orthographicSize;
        float camViewHalfWidth = camViewHalfHeight * Camera.main.aspect;
        float xMin = Camera.main.transform.position.x - camViewHalfWidth;
        float xMax = Camera.main.transform.position.x + camViewHalfWidth;
        float yMin = Camera.main.transform.position.y - camViewHalfHeight;
        float yMax = Camera.main.transform.position.y + camViewHalfHeight;

        if (transform.position.x < xMin || transform.position.x > xMax || transform.position.y < yMin || transform.position.y > yMax)
        {
            return false;
        }
        return true;
    }

    public void spawnEnemy()
    {
        if (!isVisible())
        {
            spawn = true;
        }
        else
        {
            if (spawn == true)
            {
                enemy = Instantiate(enemyPrefab);
                enemy.name = enemyPrefab.name;
                enemy.transform.position = transform.position;
                spawn = false;
            }
        }
    }




}
