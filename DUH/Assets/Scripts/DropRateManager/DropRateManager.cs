using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }
    public List<Drops> drops;

    private void OnDestroy()
    {

        DropItems();
    }

    private void DropItems()
    {
        if (!gameObject.scene.isLoaded){
            return;
        }
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();
        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        if (possibleDrops.Count > 0)
        {
            // Sort possibleDrops based on dropRate in descending order
            possibleDrops.Sort((a, b) => b.dropRate.CompareTo(a.dropRate));

            // Instantiate the item with the highest drop rate
            Drops selectedDrop = possibleDrops[0];
            Instantiate(selectedDrop.itemPrefab, transform.position, Quaternion.identity);
        }
    }
}
