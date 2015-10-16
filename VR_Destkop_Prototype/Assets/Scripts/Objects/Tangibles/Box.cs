﻿using System;
using UnityEngine;

public class Box : TangibleObject
{
    private Animator animator;
    private OpenBox openBox;
    private bool isOpen;

    public override void Start()
    {
        canContainObjects = true;
        base.Start();
        animator = GetComponent<Animator>();
        openBox = GetComponent<OpenBox>();
    }

    public override void Update()
    {
        base.Update();
        CheckOpenGesture();
    }

    private void CheckOpenGesture()
    {
        if (poseManager.GetCurrentPose() == myoMapper.handMapping.waveLeft && !isOpen)
        {
            isOpen = true;
            openBox.ShowGrid();
            animator.SetBool("IsOpen", true);
        }

        if (poseManager.GetCurrentPose() == myoMapper.handMapping.waveRight && isOpen)
        {
            isOpen = false;
            openBox.HideGrid();
            animator.SetBool("IsOpen", false);
        }
    }

    public override Renderer GetRenderer()
    {
        GameObject meshHolder = transform.FindChild("Cardboard").gameObject;
        return meshHolder.GetComponent<Renderer>();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!selected && other.gameObject.tag == ApplicationConstants.Tags.TANGIBLE)
        {
            if (other.gameObject.GetComponent<TangibleObject>().selected)
            {
                animator.SetBool("IsOpen", true);
            }
            else
            {
                GetComponent<Collider>().isTrigger = false;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // avoid other objects to be pushed into trigger indirectly
        if (!selected)
        {
            GetComponent<Collider>().isTrigger = true;
        }
    }

    public void SpawnIcons()
    {

    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        animator.SetBool("IsOpen", false);
    }

    public override void OnGrab()
    {
        ShowActionIcons(GestureIconBuilder.BuildActionHolderSet(GestureIconBuilder.ActionHolderType.MOVE_ICON));
        GetComponent<Collider>().isTrigger = false;
    }

    public override void OnRelease()
    {
        GetComponent<Collider>().isTrigger = true;
        ShowActionIcons(GestureIconBuilder.BuildActionHolderSet(GestureIconBuilder.ActionHolderType.BASIC_BOX));
    }

    public override void OnSelect()
    {
        SetEmission(ApplicationConstants.HIGHLIGHTED);
        ShowActionIcons(GestureIconBuilder.BuildActionHolderSet(GestureIconBuilder.ActionHolderType.BASIC_BOX));
    }

    public override void OnDeselect()
    {
        SetEmission(Color.black);
        HideActionIcons();
    }
}