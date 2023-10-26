using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] List<EquipmentBase> allEquips = new List<EquipmentBase>();
    [SerializeField] GameObject canvas;
    [SerializeField] List<UpgradeCard> cards = new List<UpgradeCard>();
    [SerializeField] int maxSelectionCount = 3;
    public void Trigger()
    {
        Menu.PauseLevel++;
        canvas.SetActive(true);
        foreach (var c in cards)
            c.Set(null);

        List<EquipmentBase> possibleUpgrades = new List<EquipmentBase>();
        foreach (var e in allEquips)
            if (e.NextLevelData != null)
                possibleUpgrades.Add(e);

        List<EquipmentBase> selectedUpgrades = new List<EquipmentBase>();
        for (int i = 0; i < maxSelectionCount; i++)
        {
            if (possibleUpgrades.Count == 0)
                break;
            var r = Random.Range(0, possibleUpgrades.Count);
            var selected = possibleUpgrades[r];
            possibleUpgrades.Remove(selected);
            selectedUpgrades.Add(selected);
        }
        if (selectedUpgrades.Count == 0)
        {
            Close();
            return;
        }
        for (int i = 0; i < selectedUpgrades.Count; i++)
            cards[i].Set(selectedUpgrades[i]);
    }

    public void Close()
    {
        Menu.PauseLevel--;
        canvas.SetActive(false);
    }
}