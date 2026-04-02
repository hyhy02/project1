using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Player playerData;
    //[SerializeField] float playerSpeed = 5f;
    public Vector2 input;
    public Transform cameraTransform;
    private float yVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerData = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

    }
    
    private void Move()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;

        if (controller.isGrounded)
        {
            yVelocity = -2f;
        }
        else
        {
            yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        if (inputDir.magnitude < 0.01f)
        {
            playerData.currentState = Player.PlayerState.Idle;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerData.currentState = Player.PlayerState.Run;
            }
            else
            {
                playerData.currentState = Player.PlayerState.Walk;
            }
        }

        Vector3 moveDir = Vector3.zero;

        if (inputDir.magnitude >= 0.01f)
        {
            // 카메라 기준 회전
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            float rotationSpeed = 10f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 이동 방향
            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        }
        
        // 속도
        float speed = playerData.walkSpeed;
        if (playerData.currentState == Player.PlayerState.Run)
        {
            speed = playerData.runSpeed;
        }

        // 이동
        Vector3 velocity = moveDir * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }
}
