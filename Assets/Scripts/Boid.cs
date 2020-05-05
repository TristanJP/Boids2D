﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Main controller;
    private Vector3 containerSize;
    private Renderer boidRenderer;
    private Color color;

    private Transform previousTransform;
    private Vector3 position;
    private Vector3 velocity;

    public float maxSpeed = 5.0f;
    public float minSpeed = 3.0f;
    public float steeringMaxSpeed = 5.0f;
    public float steeringMinSpeed = 3.0f;
    public int numRays = 24;
    public float rayAngle = 240.0f;
    public float rayDistance = 1.2f;
    private float boidWidth = 0.22f;


    void Awake() {

        // get container
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
        boidRenderer.material.SetColor("_Color", color);

        // Rotate to random direction
        float randomAngle = Random.Range(0f,360f);
        transform.Rotate(randomAngle, 90.0f, 0.0f, Space.Self);

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

        Vector3 collisionAvoidDirection = avoidanceDirection();

        acceleration += SteerTowards(collisionAvoidDirection);

        velocity += acceleration * Time.deltaTime;

        // get speed
        float speed = velocity.magnitude;

        // get direction
        Vector3 direction = velocity / speed;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        velocity = direction * speed;

        // Move position
        position += velocity * Time.deltaTime;

        // point in direction headed
        transform.forward = direction;
    }

    void OnTriggerExit(Collider container) {

        // Wrap Boids to Container
        Vector3 newPosition = position;

        if (position.x > container.transform.position.x + containerSize.x/2) {
            newPosition.x -= containerSize.x + 0.5f;
        }
        else if (position.x < container.transform.position.x - containerSize.x/2) {
            newPosition.x += containerSize.x + 0.5f;
        }
        if (position.y > container.transform.position.y + containerSize.y/2) {
            newPosition.y -= containerSize.y + 0.5f;

        }
        else if (position.y < container.transform.position.y - containerSize.y/2) {
            newPosition.y += containerSize.y + 0.5f;
        }

        position = newPosition;

    }

    private Vector3 avoidanceDirection() {
        RaycastHit hit;

        for (int i = 0; i < numRays; i++) {
            Vector3 rayVector = transform.rotation
                    * Quaternion.AngleAxis(-1*rayAngle/2+(i*rayAngle/numRays), Vector3.left)
                    * Vector3.forward;
            if (Physics.SphereCast(position + (transform.forward * 0.3f), boidWidth, rayVector, out hit, rayDistance))  {

                if (controller.showRays) {
                    Debug.DrawRay(position + (transform.forward * 0.3f), rayVector *  rayDistance, Color.red);
                }

                return -rayVector;

            }
            else {
                if (controller.showRays) {
                    Debug.DrawRay(position + (transform.forward * 0.3f), rayVector * rayDistance, Color.green);
                }
            }

        }
        return transform.forward;
    }

    Vector3 SteerTowards(Vector3 vector) {
        Vector3 v = vector.normalized * maxSpeed - velocity;
        return Vector3.ClampMagnitude (v, steeringMaxSpeed);
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