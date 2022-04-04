using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject ItemPrefab;
    public GameObject SpawnedItem;

    private void FixedUpdate()
    {
        if (!SpawnedItem && ItemPrefab) StartCoroutine(SpawnNewItem());
    }

    IEnumerator SpawnNewItem()
    {
        GameObject item = Instantiate(ItemPrefab, transform.position, Quaternion.identity);
        SpawnedItem = item;
        item.SetActive(false);
        yield return new WaitForSeconds(2f);
        item.SetActive(true);
    }
}
