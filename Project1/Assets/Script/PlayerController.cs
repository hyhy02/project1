using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Player playerData;
    PlayerAnimation playerAnimation;
    //[SerializeField] float playerSpeed = 5f;
    public Vector2 input;
    public Transform cameraTransform;
    private float yVelocity;
    [SerializeField] private float jumpForce = 5f;

    //콤보 공격
    private int comboStep = 0;
    private bool isAttacking = false;
    private bool canComboInput = false;
    //private bool comboInput = false;



    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerData = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public bool IsGrounded => controller.isGrounded;

    // Update is called once per frame
    void Update()
    {
        Move();
        AttackInput();
    }

    private void Move()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;

        if (controller.isGrounded)
        {
            //yVelocity = -2f;
            if (yVelocity < 0)
            {
                yVelocity = -2f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = jumpForce;
            }
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

    private void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking)
            {
                // 첫 공격 시작
                comboStep = 1;
                isAttacking = true;

                playerAnimation.PlayAttack(comboStep);
            }
            else if (canComboInput)
            {
                // 다음 콤보
                if (comboStep < 3)
                {
                    comboStep++;
                    playerAnimation.PlayAttack(comboStep);

                    canComboInput = false; // 중복 입력 방지
                }
            }
        }
    }
    public void EnableComboInput()
    {
        canComboInput = true;
        Debug.Log("콤보 입력 가능");
    }

    public void DisableComboInput()
    {
        canComboInput = false;
    }
    
    // 콤보 공격 종료
    void EndCombo()
    {
        Debug.Log("콤보 종료");
        comboStep = 0;
        isAttacking = false;
        canComboInput = false;
    }
}
