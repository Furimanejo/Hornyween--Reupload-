using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingWeapon : PeriodicWeapon
{
    [SerializeField] GameObject weaponModel;
    [SerializeField] float swingDuration = 0.5f;

    PlayerController playerController;
    float targetAngle;
    float timer = -1f;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    protected override void Trigger()
    {
        weaponModel.SetActive(true);
        timer = swingDuration;
        targetAngle = -360f;
        if (playerController.Flip())
            targetAngle *= -1f;
    }
    protected override void Update()
    {
        base.Update();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            UpdateSwingAngle();
        }
        else
        {
            timer = -1f;
            weaponModel.SetActive(false);
        }
    }
    void UpdateSwingAngle()
    {
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle * (1 - timer / swingDuration));
    }
}