using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] upgradeTexts;

      [SerializeField] private PlayerInputs player;

    private RoomManager currentRoom;

    private enum UpgradeType
    {
        Damage,
        Heal,
        AttackSpeed,
        MoveSpeed,
        MaxHealth
    }

    private List<UpgradeType> currentUpgrades = new List<UpgradeType>();

    private void Start()
    {
        upgradePanel.SetActive(false);
    }

    public void ShowUpgrades(RoomManager room)
    {
        currentRoom = room;

        GenerateRandomUpgrades();

        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void GenerateRandomUpgrades()
    {
        currentUpgrades.Clear();

        List<UpgradeType> allUpgrades = new List<UpgradeType>
        {
            UpgradeType.Damage,
            UpgradeType.Heal,
            UpgradeType.AttackSpeed,
            UpgradeType.MoveSpeed,
            UpgradeType.MaxHealth
        };

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, allUpgrades.Count);

            currentUpgrades.Add(allUpgrades[randomIndex]);
            allUpgrades.RemoveAt(randomIndex);
        }

        for (int i = 0; i < 3; i++)
        {
            upgradeTexts[i].text = GetUpgradeText(currentUpgrades[i]);

            int buttonIndex = i;
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ChooseUpgrade(buttonIndex));
        }
    }

    private string GetUpgradeText(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.Damage:
                return "Damage +5";

            case UpgradeType.Heal:
                return "Heal 30% max HP";

            case UpgradeType.AttackSpeed:
                return "Attack Speed +0.2";

            case UpgradeType.MoveSpeed:
                return "Move Speed +0.5";

            case UpgradeType.MaxHealth:
                return "Max Health +20";

            default:
                return "Upgrade";
        }
    }

    private void ChooseUpgrade(int index)
    {
        UpgradeType selectedUpgrade = currentUpgrades[index];

        switch (selectedUpgrade)
        {
            case UpgradeType.Damage:
                player.IncreaseDamage(5f);
                break;

            case UpgradeType.Heal:
                player.Heal(30f);
                break;

            case UpgradeType.AttackSpeed:
                player.IncreaseAttackSpeed(0.2f);
                break;

            case UpgradeType.MoveSpeed:
                player.IncreaseMoveSpeed(0.5f);
                break;

            case UpgradeType.MaxHealth:
                player.IncreaseMaxHealth(20f);
                break;
        }

        CloseUpgrades();
    }

    private void CloseUpgrades()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;

        if (currentRoom != null)
        {
            currentRoom.UpgradeFinished();
            currentRoom = null;
        }
    }
}