using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    [field: SerializeField] public string DisplayName { get; protected set; } = "Display Name";
    [field: SerializeField] public Sprite Icon { get; protected set; }
    [field: SerializeField] public int CurrentLevel { get; protected set; } = 0;

    [SerializeField] protected List<UpgradeData> upgradeData = new List<UpgradeData>();
    protected UpgradeData CurrentLevelData
    {
        get
        {
            if (CurrentLevel == 0)
                return null;
            return upgradeData[CurrentLevel-1];
        }
    }
    public UpgradeData NextLevelData
    {
        get
        {
            if (CurrentLevel >= upgradeData.Count)
                return null;
            return upgradeData[CurrentLevel-1+1];
        }
    }

    public void LevelUp()
    {
        CurrentLevel++;
    }

    [System.Serializable]
    public class UpgradeData
    {
        [TextArea(0,3)]
        public string description;
        public float cooldown = 10f;
        public float damage = 10f;
        public int count = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponentInParent<EnemyController>();
        if (enemy)
        {
            enemy.TakeDamage(CurrentLevelData.damage);

        }
    }
}