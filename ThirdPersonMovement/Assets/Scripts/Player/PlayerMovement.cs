using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    private Vector3 move;
    private Vector3 moveDirection;

    private bool isMove;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (!isMove)
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = move.normalized;
        }
        else
        {
            move = Vector3.zero;
        }
        

        // Walk
        if (Input.GetKey(GlobalInputs.moveForwardKey)|| 
            Input.GetKey(GlobalInputs.moveBackwardKey) || 
            Input.GetKey(GlobalInputs.moveLeftKey) || 
            Input.GetKey(GlobalInputs.moveRightKey))
        {
            animator.SetBool("Run", false);
            if (move != Vector3.zero)
            {
                animator.SetBool("Walk", true);
                characterController.Move(move * Time.deltaTime * speed);

                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
        }
        else
        {
            animator.SetBool("Walk", false);
        }

        // Run
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(GlobalInputs.moveForwardKey) ||
            Input.GetKey(GlobalInputs.moveBackwardKey) ||
            Input.GetKey(GlobalInputs.moveLeftKey) ||
            Input.GetKey(GlobalInputs.moveRightKey)))
        {
            animator.SetBool("Walk", false);
            if (move != Vector3.zero)
            {
                animator.SetBool("Run", true);
                characterController.Move(move * Time.deltaTime * speed * 2.5f);
            }
        }
        else
        {
            animator.SetBool("Run", false);
        }

        // Crouch Idle
        if (Input.GetKeyDown(GlobalInputs.crouchKey))
        {
            isMove = true;
            characterController.height = 1;
            characterController.center = new Vector3(0, 0.7f, 0);
            characterController.radius = 0.7f;
            animator.SetBool("Crouch", true);
        }
        else if(Input.GetKeyUp(GlobalInputs.crouchKey))
        {
            isMove = false;
            characterController.height = 2;
            characterController.center = new Vector3(0, 1f, 0);
            characterController.radius = 0.5f;
            animator.SetBool("Crouch", false);
        }
    }
}
