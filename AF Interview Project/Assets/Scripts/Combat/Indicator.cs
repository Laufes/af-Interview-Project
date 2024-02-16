using DG.Tweening;
using UnityEngine;

namespace AFSInterview.Combat
{
    public class Indicator : MonoBehaviour
    {
        [SerializeField] private float animationTime;
        [SerializeField] private float maxScale;
        [SerializeField] private float yOffset;

        public void Initialize()
        {
            transform.DOScale(maxScale, animationTime).SetLoops(-1, LoopType.Yoyo);
        }

        public void Show(Vector3 position)
        {
            position.y += yOffset;
            transform.position = position;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
