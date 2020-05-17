using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public bool showGreenRays = false;
    public bool showRedRays = false;
    public int boidCount = 48;
    public bool avoidCollision = true;
    public bool alignDirection = true;
    public bool steerToCentre = true;
    public GameObject boid;
    private List<Transform> boidList;

    public ColorPicker colorPickerComp;
    public Slider boidCountSlider;
    public Text boidCountText;

    // Start is called before the first frame update
    void Start()
    {
        boidList = new List<Transform>();
        int i = 0;
        while (i < boidCount) {
            addBoid();
            i += 1;
        }

        colorPickerComp.color = new Color(0.2f, 0.9f, 1f);
        boidCountSlider.value = boidCount;
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
                    randomPosition.y -= 0.5f;
                    break;
                case 2:
                    // top
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(Random.value, 1));
                    randomPosition.y += 0.5f;
                    break;
                case 3:
                    // left
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(0, Random.value));
                    randomPosition.x -= 0.5f;
                    break;
                case 4:
                    // right
                    randomPosition = Camera.main.ViewportToWorldPoint(new Vector2(1, Random.value));
                    randomPosition.x += 0.5f;
                    break;
            }

        }

        GameObject boidInstance = Instantiate(boid, randomPosition, Quaternion.Euler(0,0,Random.rotation.z));
        //Boid boidController = boidInstance.GetComponent<Boid>();
        boidList.Add(boidInstance.transform);
    }

    public void removeBoid(Transform boidToRemove) {
        boidList.Remove(boidToRemove);
        Destroy(boidToRemove.gameObject);

        if (boidList.Count < boidCount) {
            addBoid(false);
        }
    }

    public void setBoidCount(float newBoidCount) {
        boidCount = (int)newBoidCount;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform boid in boidList) {
            Boid boidController = boid.GetComponent<Boid>();
            boidController.setColor(colorPickerComp.color);
        }

        while (boidCount != boidList.Count) {
            if (boidCount < boidList.Count) {
                Transform chosenBoid = boidList[Random.Range(1, boidList.Count)];
                removeBoid(chosenBoid);
            }
            if (boidCount > boidList.Count) {
                addBoid(false);
            }
            boidCountText.text = ""+ boidCount;
        }
    }
}
