using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public bool showGreenRays = false;
    public bool showRedRays = false;
    public int boidCount = 10;
    public bool avoidCollision = true;
    public bool alignDirection = true;
    public bool steerToCentre = true;
    public GameObject boid;
    private List<Transform> boidList;

    // Start is called before the first frame update
    void Start()
    {
        boidList = new List<Transform>();
        int i = 0;
        while (i < boidCount) {
            Vector2 randomPositionOnScreen = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
            GameObject boidInstance = Instantiate(boid, randomPositionOnScreen, Quaternion.identity);
            //Boid boidController = boidInstance.GetComponent<Boid>();
            boidList.Add(boidInstance.transform);
            i += 1;
        }

    }

    public List<Transform> getNearbyBoids(Transform requestingBoid, float distance) {
        List<Transform> nearbyBoids = new List<Transform>();
        foreach (Transform boid in boidList) {
            if (boid != requestingBoid) {
                float dist = Vector3.Distance(requestingBoid.position, boid.position);
                if (dist <= distance) {
                    nearbyBoids.Add(boid);
                }
            }
        }
        return nearbyBoids;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
