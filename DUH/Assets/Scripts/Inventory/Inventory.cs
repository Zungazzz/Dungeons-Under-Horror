using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(4);
    public int[] weaponLevels = new int[4];
    public List<Image> weaponUIImages = new List<Image>(4);
    public List<Image> passiveItemUIImages = new List<Image>(4);
    public List<PassiveItems> passiveItemsSlots = new List<PassiveItems>();
    public int[] passiveItemLevels = new int[4];
    PlayerStats player;
    void Start()
    {
        player = GetComponent<PlayerStats>();
    }
    [System.Serializable]
    public class WeaponUpgrade
    {
        public int WeaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }
    [System.Serializable]
    public class PassiveItemsUpgrade
    {
        public int PassiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemsScriptableObject passiveItemsScriptableObject;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;

    }
    public List<WeaponUpgrade> weaponUpgrades = new List<WeaponUpgrade>();
    public List<PassiveItemsUpgrade> passiveItemUpgrades = new List<PassiveItemsUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUIImages[slotIndex].enabled = true;
        weaponUIImages[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem)
    {
        passiveItemsSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemsScriptableObject.Level;
        passiveItemUIImages[slotIndex].enabled = true;
        passiveItemUIImages[slotIndex].sprite = passiveItem.passiveItemsScriptableObject.Icon;

        if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
        {
            GameManager.instance.EndLevelUp();
        }
    }
    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogWarning("No next level prefab found for this weapon");
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, weapon.transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgrades[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;
            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemsSlots.Count > slotIndex)
        {
            PassiveItems passiveItem = passiveItemsSlots[slotIndex];
            if (!passiveItem.passiveItemsScriptableObject.NextLevelPrefab)
            {
                Debug.LogWarning("No next level prefab found for this passive item");
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemsScriptableObject.NextLevelPrefab, passiveItem.transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsScriptableObject.Level;

            passiveItemUpgrades[upgradeIndex].passiveItemsScriptableObject = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsScriptableObject;
            if (GameManager.instance != null && GameManager.instance.choosingUpgrade)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgrades);
        List<PassiveItemsUpgrade> availablePassiveItemUpgrades = new List<PassiveItemsUpgrade>(passiveItemUpgrades);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            int upgradeType = Random.Range(1, 3);
            if (upgradeType == 1 && availableWeaponUpgrades.Count > 0)
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.WeaponUpgradeIndex));
                                //Set the description and name of next level
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));

                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;

                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2 && availablePassiveItemUpgrades.Count > 0)
            {
                PassiveItemsUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);
                if (chosenPassiveItemUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);
                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemsSlots.Count; i++)
                    {
                        if (passiveItemsSlots[i] != null && passiveItemsSlots[i].passiveItemsScriptableObject == chosenPassiveItemUpgrade.passiveItemsScriptableObject)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                if (!chosenPassiveItemUpgrade.passiveItemsScriptableObject.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.PassiveItemUpgradeIndex));

                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemsScriptableObject.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsScriptableObject.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemsScriptableObject.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsScriptableObject.Name;
                            }
                            break;


                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItems(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemsScriptableObject.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemsScriptableObject.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemsScriptableObject.Icon;

                }

            }
        }
    }
    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }
    public void RemoveAndApplyUpgradeOptions()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }
    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}
