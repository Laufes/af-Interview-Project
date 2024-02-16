namespace AFSInterview.Items
{
    using System.Collections.Generic;
    using UnityEngine;

    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private List<Item> items;
        [SerializeField] private int money;

        public int Money => money;
        public int ItemsCount => items.Count;

        public void SellAllItemsUpToValue(int maxValue)
        {
            for (int i = 0; i < items.Count; i++)
            {
                int itemValue = items[i].Value;

                if (itemValue > maxValue)
                {
                    continue;
                }

                money += itemValue;
                items.RemoveAt(i);
                --i;
            }
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void ConsumeItem(out bool addRandomItem)
        {
            addRandomItem = false;

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items[i];

                if (item.ItemType != ItemType.Consumable)
                {
                    continue;
                }

                if (Random.value <= item.ChanceToSpawnItem)
                {
                    addRandomItem = true;
                }
                else
                {
                    money += item.Value;
                }

                items.RemoveAt(i);
                return;
            }
        }
    }
}
