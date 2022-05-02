using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright SentientDragon5 2022
 * Please Attribute
 * 
 * This is a simple class that simulates movement. attach to the player.
 */


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class SimpleChar : MonoBehaviour
{
    //public Transform directions;
    //private CharacterController c;//Saving does not work with CharacterController component.
    private Rigidbody rb;
    public float speed = 2;

    void Start()
    {
        //c = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //c.Move(CameraRelitiveMovement(MoveInput, Camera.main.transform) * speed);
        //Move(CameraRelitiveMovement(MoveInput, Camera.main.transform) * speed);
    }

    Vector3 CameraRelitiveMovement(Vector2 input, Transform cam)
    {
        Vector3 forward = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        Vector3 right = new Vector3(cam.right.x, 0, cam.right.z).normalized;
        return forward * input.y + right * input.x;
    }
    Vector2 MoveInput { get { return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); } } 

    //normally you would separate the controller and the brain.
    public void Move(Vector3 speed)
    {
        vel = speed;//+= speed / 2 * Time.deltaTime;
        
    }
    Vector3 vel = Vector3.zero;
    private void FixedUpdate()
    {
        Move(CameraRelitiveMovement(MoveInput, Camera.main.transform) * speed);

        vel = Vector3.ClampMagnitude(vel, speed);
        rb.velocity = vel;
        vel -= vel.normalized * speed / 5 * Time.deltaTime;
    }
}
