using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler ,IEndDragHandler
    {
        [SerializeField, Range(0, 1f)] private float _alphaInterval; 
        
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private int _siblingIndex;

        private float _baseAlpha;
        private Transform _slotTransform;

        protected virtual void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _canvas = GetComponentInParent<Canvas>();
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _baseAlpha = _canvasGroup.alpha;

            _slotTransform = _rectTransform.parent;
            _siblingIndex = _slotTransform.GetSiblingIndex();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            ChangeToDynamicState();
        }

        private void ChangeToDynamicState()
        {
            _slotTransform.SetAsLastSibling();
            _canvasGroup.alpha = _alphaInterval;
            ActivateBlockRaycast(false);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta/_canvas.scaleFactor;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            ChangeToStaticState();
        }

        private void ChangeToStaticState()
        {
            _slotTransform.SetSiblingIndex(_siblingIndex);
            _rectTransform.localPosition = Vector3.zero;
            _canvasGroup.alpha = _baseAlpha;
            ActivateBlockRaycast(true);
        }

        protected void ActivateBlockRaycast(bool value)
        {
            _canvasGroup.blocksRaycasts = value;
        }
    }
}