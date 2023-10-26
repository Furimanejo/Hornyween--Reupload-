using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangWeapon : PeriodicWeapon
{
    [SerializeField] float targetDistance = 10f;
    [SerializeField] float halfCycleDuration = 3f;
    [SerializeField] float angularSpeed = 90f;
    [SerializeField] GameObject model;

    PlayerController playerController;
    float timer = -1f;

    Vector3 initialPosition = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    protected override void Trigger()
    {
        model.SetActive(true);
        model.transform.localPosition = Vector3.zero;
        initialPosition = model.transform.position;
        targetPosition = transform.position + targetDistance * (Vector3)playerController.direction.normalized;
        timer = 2 * halfCycleDuration;
    }
    protected override void Update()
    {
        base.Update();
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            UpdatePositionAndRotation();
        }
        else
        {
            model.SetActive(false);
        }
    }

    void UpdatePositionAndRotation()
    {
        if (timer >= halfCycleDuration)
        {
            var t = 1 - (timer-halfCycleDuration)/ halfCycleDuration;
            model.transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
        }
        else
        {
            var t = 1 - timer / halfCycleDuration;
            model.transform.position = Vector3.Lerp(targetPosition, transform.position, t);
        }
        model.transform.rotation = Quaternion.Euler(0, 0, angularSpeed * Time.time);
    }
}