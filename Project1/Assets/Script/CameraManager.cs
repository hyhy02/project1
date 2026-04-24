using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera lockOnCam;
    public CinemachineFreeLook freeLookCam;
    public TargetingSystem targeting;

    private bool isLockOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targeting.currentTarget != null)
        {
            lockOnCam.Priority = 20;
            freeLookCam.Priority = 10;

            lockOnCam.LookAt = targeting.currentTargetPoint;

            isLockOn = true;
        }
        else
        {
            lockOnCam.Priority = 10;
            freeLookCam.Priority = 20;

            if (isLockOn)
            {
                SyncFreeLook(); // 한 번만 실행
                isLockOn = false;
            }


        }
    }

    // freelock 카메라 각도를 현재 카메라 방향으로
    void SyncFreeLook()
    {
        
    }


}
