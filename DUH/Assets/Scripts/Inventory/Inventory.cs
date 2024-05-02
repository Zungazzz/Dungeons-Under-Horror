using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(4);
    public int[] weaponLevels = new int[4];
    public List<PassiveItems> passiveItemsSlots = new List<PassiveItems>();
    public int[] passiveItemLevels = new int[4];
    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex]= weapon.weaponData.Level;
    }
    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem)
    {
        passiveItemsSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemsScriptableObject.Level;
    }
    public void LevelUpWeapon(int slotIndex)
    {
        if(weaponSlots.Count>slotIndex){
            WeaponController weapon = weaponSlots[slotIndex];
            if(!weapon.weaponData.NextLevelPrefab){
                Debug.LogWarning("No next level prefab found for this weapon");
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, weapon.transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
        }
    }
    public void LevelUpPassiveItem(int slotIndex)
    {
        if(passiveItemsSlots.Count>slotIndex){
            PassiveItems passiveItem = passiveItemsSlots[slotIndex];
            if(!passiveItem.passiveItemsScriptableObject.NextLevelPrefab){
                Debug.LogWarning("No next level prefab found for this passive item");
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemsScriptableObject.NextLevelPrefab, passiveItem.transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsScriptableObject.Level;
        }
    }
}
