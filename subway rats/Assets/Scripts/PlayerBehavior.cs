using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 10;
    
    public GameObject model;
    public Camera camera;
    
    private Rigidbody rb; //rat body
    private Animator animator;
    
    private InputAction moveAction;
    
    private Vector2 inputVector;
    private Vector3 cameraForward;
    
    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
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

    private void Move()
    {
        Vector3 frameMovement = (cameraForward * inputVector.y) +
                                (new Vector3(cameraForward.z, 0, -cameraForward.x) * inputVector.x);

        Debug.DrawRay(transform.position, frameMovement * 10, Color.red);
        
        model.transform.rotation = Quaternion.LookRotation(frameMovement, Vector3.up);
        
        if (rb.SweepTest(frameMovement, out var hit, Time.deltaTime))
        {
            frameMovement *= hit.distance;
        }
        else frameMovement *= Time.deltaTime;

        transform.position += frameMovement;

    }

}
