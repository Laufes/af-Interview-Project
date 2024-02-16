using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AFSInterview.Combat
{
    public class Tooltip : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI text;

        public CanvasScaler Canvas;

        public void ShowTooltip(Vector3 position, string description)
        {
            RectTransform rectTransform = transform as RectTransform;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            Vector2 referenceResolution = Canvas.referenceResolution;
            Vector2 localPoint = new Vector2(screenPoint.x * referenceResolution.x / Screen.width, screenPoint.y * referenceResolution.y / Screen.height);

            rectTransform.anchoredPosition = localPoint;

            text.text = description;
            gameObject.SetActive(true);
        }

        public void HideText()
        {
            gameObject.SetActive(false);
        }
    }
}
