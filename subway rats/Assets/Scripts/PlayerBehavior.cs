using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 10;

    public float sprintMultiplier = 2;
    
    public GameObject model;
    public Camera camera;
    
    private Rigidbody rb; //rat body
    private Animator animator;
    
    private InputAction moveAction;
    private InputAction sprintAction;
    
    private Vector2 inputVector;
    private Vector3 cameraForward;
    public Transform raycastPoint;
    public LayerMask groundLayer;
    public ParticleSystem dashParticles;
    
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        inputVector = moveAction.ReadValue<Vector2>();
        cameraForward = camera.transform.forward;
        cameraForward = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

        if (inputVector.sqrMagnitude > 0.01f)
        {
            Move();
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    private int dashCounter = 0;
    
    private void Move()
    {
        Vector3 frameMovement = (cameraForward * inputVector.y) +
                                (new Vector3(cameraForward.z, 0, -cameraForward.x) * inputVector.x);

        //Debug.DrawRay(transform.position, frameMovement * 10, Color.red);

        RaycastHit groundRay;
        bool forwardHit = Physics.Raycast(transform.position + (Vector3.up * 0.15f), Vector3.down, out groundRay, Mathf.Infinity, groundLayer.value);


        if (forwardHit)
        {
            print(groundRay.transform.name);
        
            transform.position = new Vector3(transform.position.x, groundRay.point.y, transform.position.z);

            model.transform.rotation = Quaternion.Lerp(model.transform.rotation,
                Quaternion.LookRotation(frameMovement, groundRay.normal),
                0.5f);
        }

        frameMovement *= Time.deltaTime * moveSpeed;

        if (sprintAction.IsPressed())
        {
            //if(dashCounter == 0) dashParticles.Play();
            // if(dashCounter < 15)
            // {
            //     dashCounter++;
            //     frameMovement *= sprintMultiplier;
            // }
            frameMovement *= sprintMultiplier;
        }
        else dashCounter = 0;
        
        if (rb.SweepTest(frameMovement, out var hit, frameMovement.magnitude))
        {
            frameMovement = frameMovement.normalized * hit.distance;
        }
        
        transform.position += frameMovement;

    }

}
