using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KomaruDefeat : MonoBehaviour
{
    public float explosionSpeed = 75f;
    GameObject[] explosion = new GameObject[12];
    Vector3[] explosionVectors =
    {
        new Vector3(-100f, 0, 0),
        new Vector3(100f, 0, 0),
        new Vector3(0, -100f, 0),
        new Vector3(0, 100f, 0),
        new Vector3(-75f, -75f, 0),
        new Vector3(-75f, 75f, 0),
        new Vector3(75f, -75f, 0),
        new Vector3(75f, 75f, 0),
        new Vector3(-50f, 0, 0),
        new Vector3(50f, 0, 0),
        new Vector3(0, -50f, 0),
        new Vector3(0, 50f, 0)
    };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            string explosionName = "Explosion" + (i + 1).ToString();
            explosion[i] = transform.Find(explosionName).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            Vector3 position = explosion[i].transform.position;
            position.x += explosionVectors[i].x * explosionSpeed * Time.deltaTime;
            position.y += explosionVectors[i].y * explosionSpeed * Time.deltaTime;
            position.z += explosionVectors[i].y * explosionSpeed * Time.deltaTime;
            explosion[i].transform.position = position;

        }

    }
}
