using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyType : ScriptableObject
{
    [field: SerializeField] public float Health { get; private set; } = 10f;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 1f;
    [field: SerializeField] public float Damage { get; private set; } = 1f;
    [field: SerializeField] public GameObject Drop { get; private set; } = null;

    [field: SerializeField] public List<Sprite> WalkAnimSprites { get; private set; }
}
