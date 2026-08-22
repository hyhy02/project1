using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동")]
    public Vector2 input;
    // 점프
    private float yVelocity;
    [SerializeField] private float jumpForce = 5f;

    [Header("스테미나")]
    // 스테미나
    [SerializeField] private float staminaDecreaseRate = 15f; // 초당 감소
    [SerializeField] private float staminaRecoveryRate = 10f; // 초당 회복
    [SerializeField] private float jumpStaminaCost = 20f; // 점프 소모량

    private bool canRun = true;
    [SerializeField] private float runEnable = 20f; // 다시 달릴 수 있는 최소 스테미나

    //ㅁ[Header("공격")]
    //콤보 공격
    private bool isAttacking = false;
    private bool canComboInput = false;
    private int comboStep = 0;

    [Header("가드")]
    //가드
    public bool isGuard;
    [SerializeField] private float guardDamageReduction = 0.0f; // 100% 감소

    [Header("참조")]
    [SerializeField] private Transform cameraTransform;
    private CharacterController controller;
    private Player playerData;
    private PlayerAnimation playerAnimation;
    private TargetingSystem targeting;
    [SerializeField] private GameUIManager gameUI;
    // 검
    [SerializeField] private Sword sword;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerData = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
        targeting = GetComponent<TargetingSystem>();
    }

    public bool IsGrounded => controller.isGrounded;

    // Update is called once per frame
    void Update()
    {
        Move();
        AttackInput();
        GuardInput();

        HandleStemina();

        if (targeting.currentTarget != null)
        {
            RotateToTarget();
        }

        //테스트용
        // if(Input.GetKeyDown(KeyCode.T))
        // {
        //     Die();
        // }
    }

    private void Move()
    {
        // 공격 중 일때 이동하지 못하게
        if (isAttacking || playerData.currentState==Player.PlayerState.Die)
        {
            // 공중이면 중력만 적용
            if (!controller.isGrounded && playerData.currentState!=Player.PlayerState.Die)
            {
                yVelocity += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                if (yVelocity < 0)
                    yVelocity = -2f;
            }

            Vector3 attack_velocity = Vector3.zero;
            attack_velocity.y = yVelocity;

            controller.Move(attack_velocity * Time.deltaTime);
            return;
        }

        // 입력
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;


        // 중력
        if (controller.isGrounded)
        {
            //yVelocity = -2f;
            if (yVelocity < 0)
            {
                yVelocity = -2f;
            }
            // 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(playerData.currentStamina >= jumpStaminaCost)
                {              
                    yVelocity = jumpForce;
                    playerData.currentState = Player.PlayerState.Jump;

                    playerData.currentStamina -= jumpStaminaCost;
                }
  
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
            // 달리기
            if (Input.GetKey(KeyCode.LeftShift) && !isGuard && !isAttacking && canRun)
            {
                playerData.currentState = Player.PlayerState.Run;
            }
            else
            {
                playerData.currentState = Player.PlayerState.Walk;
            }
        }
        // 스테미너 0일 때 Walk로 전환
        if (playerData.currentStamina <= 0)
        {
            playerData.currentState = Player.PlayerState.Walk;
        }

        Vector3 moveDir = Vector3.zero;

        if (inputDir.magnitude >= 0.01f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            if (targeting.currentTarget == null )//&& !suppressRotation)
            {
                // 카메라 기준 회전
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                float rotationSpeed = 6f;
                // 가드일 때 회전속도 느리게
                if (isGuard)
                {
                    rotationSpeed = 3f;
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // 이동 방향
            if (targeting.currentTarget != null)
            {
                moveDir = (transform.forward * input.y + transform.right * input.x).normalized;
            }
            else
            {
                moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            }
   
        }

        // 속도
        float speed = playerData.walkSpeed;
        if (playerData.currentState == Player.PlayerState.Run)
        {
            speed = playerData.runSpeed;
        }
        //가드일 때 속도 느리게
        if(isGuard)
        {
            speed *= 0.5f;
        }

        // 이동
        Vector3 velocity = moveDir * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    // 콤보 공격
    private void AttackInput()
    {
        if (isGuard)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking && controller.isGrounded)
            {
                // 첫 공격 시작
                comboStep = 1;
                isAttacking = true;

                playerData.currentState = Player.PlayerState.Attack;

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
        //Debug.Log("콤보 입력 가능");
    }

    public void DisableComboInput()
    {
        canComboInput = false;
        playerData.currentState = Player.PlayerState.Idle;
    }

    // 콤보 공격 종료
    void EndCombo()
    {
        //Debug.Log("콤보 종료");
        comboStep = 0;
        isAttacking = false;
        canComboInput = false;
    }

    // 검 콜라이더 보이게
    public void EnableCollider()
    {
        sword.EnableCollider();
    }
    // 검 콜라이더 안보이게
    public void DisableCollider()
    {
        sword.DisableCollider();
    }

    // 가드
    private void GuardInput()
    {
        if (!isAttacking)
        {
            isGuard = Input.GetMouseButton(1);
        }

    }

    // 피격
    public void TakeDamage(float damage, Transform attacker)
    {
        bool isFront = IsFrontAttacker(attacker);

        // 가드 중일 때 데미지 반감
        if (isGuard && isFront)
        {
            damage *= guardDamageReduction;
            playerAnimation.PlayGuardHit(); // 가드 중 피격 애니메이션
        }
        else
        {
            playerAnimation.PlayHit(); // 피격 애니메이션
        }

        playerData.currentHP -= damage;

        playerData.currentHP = Mathf.Max(playerData.currentHP, 0);

        if (playerData.currentHP == 0)
        {
            Die();
        }

        EndCombo();
    }
    
    // 앞 뒤 판정 함수
    private bool IsFrontAttacker(Transform attacker)
    {
        Vector3 dirToEnemy = (attacker.position - transform.position).normalized;

        float dot = Vector3.Dot(transform.forward, dirToEnemy); // 1: 정면, 0: 옆, -1: 완전 뒤

        return dot > 0.5f;
    }
    // 플레이어 죽음
    private void Die()
    {
        //Debug.Log("Die");
        playerData.currentState = Player.PlayerState.Die;
        playerAnimation.PlayDeath(); // 죽는 애니메이션

        gameUI.GameOver();
    }

    // 스테미너
    public void HandleStemina()
    {
        // Run ->감소
        if (playerData.currentState == Player.PlayerState.Run)
        {
            playerData.currentStamina -= staminaDecreaseRate * Time.deltaTime;
        }
        else
        {
            // Idle, Walk -> 회복
            playerData.currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        playerData.currentStamina = Mathf.Clamp(playerData.currentStamina, 0, playerData.maxStamina);

        // 스테미나 0 되면 달리기 금지
        if (playerData.currentStamina <= 0)
        {
            canRun = false;
        }

        // 일정량 회복되면 다시 달리기 가능
        if (playerData.currentStamina >= runEnable)
        {
            canRun = true;
        }
    }

    // 타겟 쪽으로 회전
    void RotateToTarget()
    {
        Vector3 dir = targeting.currentTargetPoint.position - transform.position;
        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 10f);
    }
}
