using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UGUICUSTOM
{
    public class SimpleList : MonoBehaviour
    {
        public SimpleListOption SimpleListOption;
        public GameObject Element;
        public ElementEvent ElementEvent;
        private List<GameObject> elements = new List<GameObject>();
        protected RectTransform rectTransform;
        protected BaseScrollView scrollView;
        public void Initalize()
        {
            SimpleListOption = new SimpleListOption
            {
                contentLayoutFilter = SimpleListContentLayoutFilter.None
            };
            if (!gameObject.TryGetComponent(out RectTransform rt))
            {
                rectTransform = gameObject.AddComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(200, 200);
            }
            scrollView = new BaseScrollView(this.transform);
        }
        public void OnReImport()
        {
            if (scrollView.IsUnityNull()) scrollView = new BaseScrollView(this.transform, true);
            RectTransform contentRT = scrollView.scrollRect.content;
            if (elements.Count == 0 && contentRT.childCount > 0)
            {
                for (int i = 0; i < contentRT.childCount; i++)
                {
                    elements.Add(contentRT.GetChild(i).gameObject);
                }
            }
        }
        public void OnCreateElement()
        {
            GameObject go = Instantiate(Element, scrollView.scrollRect.content);
            if (SimpleListOption.cellSize.x == 0 && SimpleListOption.cellSize.y == 0)
            {
                SimpleListOption.cellSize = go.GetComponent<RectTransform>().sizeDelta;
            }
            elements.Add(go);
        }
        public void OnClearElements()
        {
            if (scrollView.scrollRect.content.childCount > 0 && elements.Count <= 0)
            {
                for (int i = 0; i < scrollView.scrollRect.content.childCount; i++)
                {
                    elements.Add(scrollView.scrollRect.content.GetChild(i).gameObject);
                }
            }
            elements.ForEach(a => { DestroyImmediate(a); });
            elements.Clear();
        }
        public void OnChangedLayoutFilter()
        {
            switch (SimpleListOption.contentLayoutFilter)
            {
                case SimpleListContentLayoutFilter.None:
                    scrollView.NoneContentLayout();
                    break;
                case SimpleListContentLayoutFilter.Horizontal:
                    scrollView.HorizontalContentLayout();
                    break;
                case SimpleListContentLayoutFilter.Vertical:
                    scrollView.VerticalContentLayout();
                    break;
                case SimpleListContentLayoutFilter.Grid:
                    scrollView.GridContentLayout();
                    break;
            }
            OnChangeLayoutOption();
            Canvas.ForceUpdateCanvases();
        }
        public void OnChangeLayoutOption()
        {
            scrollView.ScrollRectPadding(SimpleListOption, elements);
        }

        public void OnChangeHorizontal(float value)
        {
            scrollView.scrollRect.horizontalScrollbar.value = value;
        }
        public void OnChangeVertical(float value)
        {
            scrollView.scrollRect.verticalScrollbar.value = value;
        }

    }


}
