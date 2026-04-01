using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float playerSpeed = 5f;
    public Vector2 input;

    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
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

        if (inputDir.magnitude < 0.01f)
            return;

        // 카메라 기준 회전
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        float rotationSpeed = 10f;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 이동 방향
        Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        // 이동
        controller.Move(moveDir * playerSpeed * Time.deltaTime);
    }
}
