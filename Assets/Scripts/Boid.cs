using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Main controller;
    public Light pointLight;
    private Vector3 containerSize;
    private Renderer boidRenderer;
    private Color color;

    private Transform previousTransform;
    private Vector3 position;
    private Vector3 velocity;
    List<Transform> nearbyBoids;


    public float maxSpeed = 5.0f;
    public float minSpeed = 1.0f;
    public float steeringMaxSpeed = 10.0f;
    public float steeringMinSpeed = 5.0f;
    public int numRays = 30;
    public float rayAngle = 300.0f;
    public float rayDistance = 1.2f;
    public float collisionAvoidanceWeight = 1f;
    public float alignmentDistance = 3.0f;
    public float alignmentWeight = 1f;
    public float steerToCentreWeight = 1.0f;
    private float boidWidth = 0.22f;
    private bool outside = false;


    void Awake() {

        // get controller
        controller = GameObject.Find("Main").GetComponent<Main>();

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
        setColor(color);

        // Rotate to random direction
        float randomAngle = Random.Range(0f,360f);
        transform.Rotate(randomAngle, -90.0f, 0.0f, Space.Self);

        // set position
        position = transform.position;
        // set velocity
        velocity = transform.forward * (maxSpeed + minSpeed)/2;
    }

    // Update is called once per frame
    void Update() {
        Vector3 acceleration = Vector3.zero;

        // Store last position
        previousTransform.position = position;

        // If in container, do collision avoidance
        if (!outside) {
            nearbyBoids = controller.getNearbyBoids(this, alignmentDistance);

            Vector3 newDirection = Vector3.zero;
            if (controller.avoidCollision) {
                Vector3 collisionAvoidDirection = avoidanceDirection();
                newDirection += collisionAvoidDirection * collisionAvoidanceWeight;
            }
            if (controller.alignDirection) {
                Vector3 alignmentVector = getFlockAlignment();
                newDirection += alignmentVector * alignmentWeight;
            }
            if (controller.steerToCentre) {
                Vector3 flockCentreDirection = getFlockCentre() - position;
                newDirection += flockCentreDirection.normalized * steerToCentreWeight;
            }
            acceleration += SteerTowards(newDirection);

            velocity += acceleration * Time.deltaTime;
        }


        velocity = new Vector3(velocity.x, velocity.y, 0f);

        // get speed
        float speed = velocity.magnitude;

        // get direction
        Vector3 direction = velocity / speed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        velocity = direction * speed;

        // Move position
        position += velocity * Time.deltaTime;

        // point in direction headed
        transform.rotation = Quaternion.LookRotation(direction, Vector3.left);
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Container") {
            outside = true;
            // Wrap Boids to Container
            Vector3 newPosition = position;

            if (position.x >= other.transform.position.x + containerSize.x/2) {
                newPosition.x = -(containerSize.x/2);
            }
            else if (position.x <= other.transform.position.x - containerSize.x/2) {
                newPosition.x = (containerSize.x/2);
            }
            if (position.y >= other.transform.position.y + containerSize.y/2) {
                newPosition.y = -(containerSize.y/2);
            }
            else if (position.y <= other.transform.position.y - containerSize.y/2) {
                newPosition.y = (containerSize.y/2);
            }
            position = newPosition;
        }
        else if (other.tag == "Bounds") {
            controller.removeBoid(this);
        }

    }

    void OnTriggerEnter(Collider container) {
        outside = false;
    }

    private Vector3 avoidanceDirection() {
        RaycastHit hit;
        List<Vector3> obstDirections = new List<Vector3>();

        for (int i = 0; i < numRays; i++) {
            Vector3 rayVector = transform.rotation
                    * Quaternion.AngleAxis(1*rayAngle/2-(i*rayAngle/numRays), Vector3.left)
                    * Vector3.forward;
            if (Physics.SphereCast(position + (transform.forward * 0.3f), boidWidth, rayVector, out hit, rayDistance))  {
                obstDirections.Add(rayVector);
                if (controller.showRedRays) {
                    Debug.DrawRay(position + (transform.forward * 0.3f), rayVector *  rayDistance, Color.red);
                }
            }
            else {
                if (controller.showGreenRays) {
                    Debug.DrawRay(position + (transform.forward * 0.3f), rayVector * rayDistance, Color.green);
                }
            }
        }

        if (obstDirections.Count > 0) {
            Vector3 averageVector = Vector3.zero;
            foreach (Vector3 dir in obstDirections) {
                averageVector += dir.normalized;
            }
            return averageVector.normalized*-1;
        }
        return transform.forward;
    }

    private Vector3 getFlockCentre() {
        Vector3 centre = Vector3.zero;
        foreach (Transform boidT in nearbyBoids) {
            centre += boidT.position;
        }
        return centre/nearbyBoids.Count;
    }

    private Vector3 getFlockAlignment() {
        Vector3 alignment = Vector3.zero;
        foreach (Transform boidT in nearbyBoids) {
            alignment += boidT.forward;
        }
        return alignment.normalized;
    }

    private Vector3 SteerTowards(Vector3 vector) {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude(v, steeringMaxSpeed);
    }

    public void setColor(Color color) {
        boidRenderer.material.SetColor("_Color", color);
        pointLight.color = color;
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
