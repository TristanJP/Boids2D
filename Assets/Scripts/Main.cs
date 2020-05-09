using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject colorPicker;
    ColorPicker colorPickerComp;
    public GameObject imageTest;
    Image imageTestComp;

    // Start is called before the first frame update
    void Start()
    {
        boidList = new List<Transform>();
        int i = 0;
        while (i < boidCount) {
            addBoid();
            i += 1;
        }

        colorPickerComp = colorPicker.GetComponent<ColorPicker>();
        colorPickerComp.color = Color.red;
        imageTestComp = imageTest.GetComponent<Image>();
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

    public void addBoid(bool onScreen = true) {
        Vector2 randomPosition = Vector2.zero;
        if (onScreen) {
            randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, Random.value));
        }
        else {
            switch (Random.Range(1,5)) {
                case 1:
                    // bottom
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, 0));
                    break;
                case 2:
                    // top
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, 1));
                    break;
                case 3:
                    // left
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, Random.value));
                    break;
                case 4:
                    // right
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, Random.value));
                    break;
            }

        }

        GameObject boidInstance = Instantiate(boid, randomPosition, Quaternion.identity);
        //Boid boidController = boidInstance.GetComponent<Boid>();
        boidList.Add(boidInstance.transform);
    }

    public void removeBoid(Transform boidToRemove) {
        boidList.Remove(boidToRemove);
    }

    // Update is called once per frame
    void Update()
    {
        imageTestComp.color = colorPickerComp.color;
        foreach (Transform boid in boidList) {
            Boid boidController = boid.GetComponent<Boid>();
            boidController.setColor(colorPickerComp.color);
        }
    }
}
