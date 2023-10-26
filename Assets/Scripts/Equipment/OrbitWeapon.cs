using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitWeapon : EquipmentBase
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] float orbitFrequency = 1f;
    [SerializeField] float orbitDistance = 2f;
    void Update()
    {
        var data = CurrentLevelData;
        if (data == null)
            return;

        var count = data.count;
        var angleBetweenObjects = 360 / count;
        var angleOffset = Time.time * 360 * orbitFrequency;
        for(int i = 0; i < count; i++)
        {
            objects[i].SetActive(true);
            var angle = angleOffset + i * angleBetweenObjects;
            angle = Mathf.Deg2Rad * angle;
            var position = orbitDistance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            objects[i].transform.localPosition = position;
        }
    }
}