using UnityEngine;

public class Altar : MonoBehaviour
{
    public ItemData DesiredItem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            GameObject itemGo = collision.gameObject.transform.parent.gameObject;
            Item item = itemGo.GetComponent<Item>();
            if (item.Name != DesiredItem.ItemName) return;
            Destroy(collision.gameObject.transform.parent.gameObject);
            GameManager.Instance.WinDay();
        }
    }
}
