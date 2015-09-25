﻿using UnityEngine;
using System.Collections;
using System;

public class SelectionManager : MonoBehaviour
{
    public GameObject armReference;
    private Transform armTransform;

    // rotation-offset from myo transform
    private Quaternion antiYaw = Quaternion.identity;
    private Vector3 myoDirection;

    // remember last selected object
    private GameObject selectedObject;
    private RaycastHit hit;

    private PoseManager poseManager;

    void Start()
    {
        poseManager = PoseManager.GetInstance();
        armTransform = armReference.transform;
    }

    void Update()
    {
        UpdateRotation();
        HandleSelection();
    }

    private void HandleSelection()
    {
        if (Physics.Raycast(armTransform.position, armTransform.forward, out hit))
        {
            if (selectedObject != null)
            {
                return;
            }
            selectedObject = hit.transform.gameObject;
        }
        else
        {
            if (selectedObject != null)
            {
                selectedObject = null;
            }
        }
    }

    private void UpdateRotation()
    {
        myoDirection = poseManager.GetCurrentDirection();
        // reset position of cursor
        if (Input.GetKeyDown(KeyCode.Space) || poseManager.GetCurrentPose() == ApplicationConstants.DefaultPose.RESET)
        {
            antiYaw = Quaternion.FromToRotation(myoDirection, Camera.main.transform.forward);
        }

        armTransform.rotation = antiYaw * Quaternion.LookRotation(myoDirection);
    }
}
