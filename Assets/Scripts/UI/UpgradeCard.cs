using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] UpgradeWindow upgradeWindow;
    [SerializeField] TMP_Text displayName;
    [SerializeField] TMP_Text description;
    [SerializeField] Image icon;
    EquipmentBase equipmentSet = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        equipmentSet.LevelUp();
        upgradeWindow.Close();
    }

    public void Set(EquipmentBase e)
    {
        equipmentSet = e;
        if (e == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            var levelString = e.CurrentLevel == 0 ? "New" : $"Level {e.CurrentLevel+1}";
            displayName.text = $"{e.DisplayName} ({levelString}) ";
            description.text = e.NextLevelData.description;
            icon.sprite = e.Icon;
        }
    }
}