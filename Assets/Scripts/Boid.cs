using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 containerSize;
    private Renderer boidRenderer;
    private Color color;

    private Transform previousTransform;
    private Vector3 position;
    private Vector3 velocity;

    public float speed = 5.0f;


    void Awake() {
        // get container
        containerSize = GameObject.Find("Container").GetComponent<Collider>().bounds.size;

        // get renderer
        boidRenderer = transform.Find("ConeBoid").GetComponent<Renderer>();
        previousTransform = transform;
    }

    // Start is called before the first frame update
    void Start() {
        // Set colour
        color = getRandomColour();
        boidRenderer.material.SetColor("_Color", color);

        // Rotate to random direction
        transform.Rotate(Random.Range(0f,360f), 90.0f, 0.0f, Space.Self);

        // set position
        position = transform.position;
        // set velocity
        velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update() {
        //float speed = velocity.magnitude;
        //Vector3 direction = velocity/speed;
        //speed = Mathf.Clamp(speed, 5.0f, 6.0f);
        //velocity = direction * speed;

        previousTransform.position += velocity * Time.deltaTime;
        position = previousTransform.position;
        //forward = transform.forward;

    }

    void OnTriggerExit(Collider container) {
        Vector3 newPosition = previousTransform.position;
        //print(containerSize.x + ", " + containerSize.y);
        if (position.x > container.transform.position.x + containerSize.x/2) {
            newPosition.x -= containerSize.x;
        }
        else if (position.x < container.transform.position.x - containerSize.x/2) {
            newPosition.x += containerSize.x;
        }
        if (position.y > container.transform.position.y + containerSize.y/2) {
            newPosition.y -= containerSize.y;

        }
        else if (position.y < container.transform.position.y - containerSize.y/2) {
            newPosition.y += containerSize.y;
        }

        previousTransform.position = newPosition;

        print(name + " EXITTED");
    }


    // Gets a random colour from a gradient
    private Color getRandomColour() {
        Gradient gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = new Color(0.32f, 1f, 0.88f);
        colorKey[0].time = 0.0f;
        colorKey[1].color = new Color(0f, 0.18f, 0.4f);
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient.Evaluate(Random.Range(0f,1f));
    }
}
