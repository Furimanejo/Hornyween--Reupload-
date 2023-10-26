using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] WaveController waveController;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject xpBar;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text timer;

    void Update()
    {
        var healthPercentage = Mathf.Max(0f, player.Health / player.MaxHealth);
        healthBar.transform.localScale = new Vector3(healthPercentage, 1f, 1f);

        var xpPercentage = Mathf.Max(0f, (float) player.XP / player.XPToNextLevel);
        xpBar.transform.localScale = new Vector3(xpPercentage, 1f, 1f);

        level.text = $"Level {player.Level}";

        int seconds = (int) waveController.timer;
        timer.text = string.Format("{0:0}:{1:00}", seconds/60 , seconds%60);

        if (healthPercentage <= 0f)
        {
            waveController.enabled = false;
            timer.text += " - Game Over";
        }
    }
}