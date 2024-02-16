namespace AFSInterview.Items
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class ItemsManager : MonoBehaviour
    {
        [SerializeField] private InventoryController inventoryController;
        [SerializeField] private int itemSellMaxValue;
        [SerializeField] private Transform itemSpawnParent;
        [SerializeField] private ItemPresenter itemPrefab;
        [SerializeField] private ItemPresenter consumableItemPrefab;
        [SerializeField] private BoxCollider itemSpawnArea;
        [SerializeField] private float itemSpawnInterval;
        [Range(0f, 1f)]
        [SerializeField] private float chanceToSpawnConsumable;
        [Space(10)]
        [SerializeField] private TextMeshProUGUI moneyText;
        [Space(10)]
        [SerializeField] private List<Item> randomItemsFromConsumables = new List<Item>();
        [SerializeField] private List<IItemHolder> itemsPools = new List<IItemHolder>();
        [SerializeField] private List<IItemHolder> consumableItemsPool = new List<IItemHolder>();

        private Dictionary<ItemType, List<IItemHolder>> itemsPool = new Dictionary<ItemType, List<IItemHolder>>();

        private float nextItemSpawnTime;

        private void Awake()
        {

        }

        private void Update()
        {
            if (Time.time >= nextItemSpawnTime)
            {
                SpawnNewItem();
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryPickUpItem();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                inventoryController.SellAllItemsUpToValue(itemSellMaxValue);
                UpdateMoneyText();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                inventoryController.ConsumeItem(out bool addRandomItem);
                if (addRandomItem)
                {
                    inventoryController.AddItem(randomItemsFromConsumables[Random.Range(0, randomItemsFromConsumables.Count)]);
                }

                UpdateMoneyText();
            }
        }

        private void UpdateMoneyText()
        {
            moneyText.text = "Money: " + inventoryController.Money;
        }

        private void SpawnNewItem()
        {
            nextItemSpawnTime = Time.time + itemSpawnInterval;

            var spawnAreaBounds = itemSpawnArea.bounds;
            var position = new Vector3(
                Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
                0f,
                Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
            );

            IItemHolder itemPresenter = GetItemPresenter(Random.value > chanceToSpawnConsumable ? ItemType.Default : ItemType.Consumable);
            itemPresenter.SetPosition(position);
            itemPresenter.Show();
        }

        private IItemHolder GetItemPresenter(ItemType itemType)
        {
            if (itemsPool.TryGetValue(itemType, out List<IItemHolder> pool) && pool.Count != 0)
            {
                IItemHolder presenter = pool[0];
                pool.RemoveAt(0);
                return presenter;
            }

            ItemPresenter prefab = itemType == ItemType.Consumable ? consumableItemPrefab : itemPrefab;
            return Instantiate(prefab, itemSpawnParent);
        }

        private void BackToPool(IItemHolder itemHolder)
        {
            if (!itemsPool.TryGetValue(itemHolder.GetItemType(), out List<IItemHolder> pool))
            {
                pool = new List<IItemHolder>();
                itemsPool.Add(itemHolder.GetItemType(), pool);
            }

            pool.Add(itemHolder);
        }

        private void TryPickUpItem()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var layerMask = LayerMask.GetMask("Item");
            if (!Physics.Raycast(ray, out var hit, 100f, layerMask) || !hit.collider.TryGetComponent<IItemHolder>(out var itemHolder))
                return;

            Item item = itemHolder.GetItem(true);
            BackToPool(itemHolder);

            inventoryController.AddItem(item);

            Debug.Log("Picked up " + item.Name + " with value of " + item.Value + " and now have " + inventoryController.ItemsCount + " items");
        }
    }
}
