using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool showRays = false;
    public int boidCount = 10;
    public GameObject boid;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        while (i < boidCount) {
            Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            Instantiate(boid, randomPositionOnScreen, Quaternion.identity);
            i += 1;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
