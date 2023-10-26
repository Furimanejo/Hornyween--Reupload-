using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : PeriodicWeapon
{
    [SerializeField] GameObject dropObject;
    Vector3 position = Vector3.zero;
    protected override void Trigger()
    {
        dropObject.SetActive(true);
        position = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        {
            dropObject.transform.position = position;
        }
    }
}
