namespace AFSInterview.Items
{
    using UnityEngine;

    public class ItemPresenter : MonoBehaviour, IItemHolder
    {
        [SerializeField] private Item item;

        public Item GetItem(bool disposeHolder)
        {
            if (disposeHolder)
            {
                gameObject.SetActive(false);
            }

            return item;
        }

        public ItemType GetItemType()
        {
            return item.ItemType;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
    }
}