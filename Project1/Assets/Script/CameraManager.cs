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
            if(!isLockOn)
            {
                isLockOn = true;

                lockOnCam.LookAt = targeting.currentTargetPoint;

                lockOnCam.Priority = 20;
                freeLookCam.Priority = 10;
            }
        }
        else
        {
            if(isLockOn)
            {
                isLockOn = false;

                lockOnCam.Priority = 10;
                freeLookCam.Priority = 20;
            }
        }
    }



}
