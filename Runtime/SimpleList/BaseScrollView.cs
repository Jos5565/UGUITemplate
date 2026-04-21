using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UGUICUSTOM
{
    [System.Serializable]
    public class BaseScrollView
    {
        public ScrollRect scrollRect;
        protected LayoutGroup layoutGroup;
        protected RectTransform viewPort;
        protected RectTransform content;
        protected Scrollbar horizontalBar;
        protected Scrollbar verticalBar;
        public BaseScrollView(Transform target, bool reImport = false)
        {
            if (reImport)
            {
                scrollRect = target.GetComponentInChildren<ScrollRect>();
                viewPort = scrollRect.viewport;
                content = scrollRect.content;
                horizontalBar = scrollRect.horizontalScrollbar;
                verticalBar = scrollRect.verticalScrollbar;
                return;
            }
            //Create Scroll View
            GameObject scv = new GameObject("Scroll View");
            scv.transform.SetParent(target);
            scv.AddComponent<Image>();
            scrollRect = scv.AddComponent<ScrollRect>();
            FullStretch(scv.GetComponent<RectTransform>());
            //Create ViewPort, Content
            ScrollViewPort(scv.transform);
            //Create Scroll Bars
            ScrollBars(scv.transform);

            //Setting
            scrollRect.content = content;
            scrollRect.viewport = viewPort;
            scrollRect.horizontalScrollbar = horizontalBar;
            scrollRect.verticalScrollbar = verticalBar;

        }
        protected void ScrollViewPort(Transform scv)
        {
            GameObject vp = new GameObject("Viewport");
            vp.transform.SetParent(scv.transform);
            vp.AddComponent<Image>();
            vp.AddComponent<Mask>().showMaskGraphic = false;
            viewPort = vp.GetComponent<RectTransform>();
            FullStretch(viewPort);

            GameObject ct = new GameObject("Content");
            ct.transform.SetParent(vp.transform);
            content = ct.AddComponent<RectTransform>();
            Top_Left(content);

        }
        protected void ScrollBars(Transform scv)
        {
            //Horizontal
            GameObject hsb = new GameObject("Horizontal ScrollBar");
            hsb.transform.SetParent(scv.transform);
            hsb.AddComponent<Image>();
            horizontalBar = hsb.AddComponent<Scrollbar>();
            HorizontalStretch(hsb.GetComponent<RectTransform>());
            horizontalBar.direction = Scrollbar.Direction.LeftToRight;
            //Slide
            GameObject hslide = new GameObject("Sliding Area");
            hslide.transform.SetParent(hsb.transform);
            FullStretch(hslide.AddComponent<RectTransform>());
            //Handle
            GameObject handle = new GameObject("Handle");
            handle.transform.SetParent(hslide.transform);
            handle.AddComponent<Image>().color = Color.black;
            horizontalBar.targetGraphic = handle.GetComponent<Image>();
            horizontalBar.handleRect = handle.GetComponent<RectTransform>();
            FullStretch(handle.GetComponent<RectTransform>());

            //Vertical
            GameObject vsb = new GameObject("Vertical ScrollBar");
            vsb.transform.SetParent(scv.transform);
            vsb.AddComponent<Image>();
            verticalBar = vsb.AddComponent<Scrollbar>();
            VerticalStretch(vsb.GetComponent<RectTransform>());
            verticalBar.direction = Scrollbar.Direction.BottomToTop;
            //Slide
            GameObject vslide = new GameObject("Sliding Area");
            vslide.transform.SetParent(vsb.transform);
            FullStretch(vslide.AddComponent<RectTransform>());
            //Handle
            GameObject vhandle = new GameObject("Handle");
            vhandle.transform.SetParent(vslide.transform);
            vhandle.AddComponent<Image>().color = Color.black;
            verticalBar.targetGraphic = vhandle.GetComponent<Image>();
            verticalBar.handleRect = vhandle.GetComponent<RectTransform>();
            FullStretch(vhandle.GetComponent<RectTransform>());
        }
        protected void Top_Left(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.one;
        }
        protected void FullStretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
        protected void VerticalStretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(10, 0);
        }
        protected void HorizontalStretch(RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = new Vector2(1, 0);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(0, 10);
        }

        public void NoneContentLayout()
        {
            if (content.TryGetComponent(out HorizontalLayoutGroup hg)) UnityEngine.Component.DestroyImmediate(hg);
            if (content.TryGetComponent(out VerticalLayoutGroup vg)) UnityEngine.Component.DestroyImmediate(vg);
            if (content.TryGetComponent(out GridLayoutGroup gg)) UnityEngine.Component.DestroyImmediate(gg);
            if (content.TryGetComponent(out ContentSizeFitter csf)) UnityEngine.Component.DestroyImmediate(csf);
        }
        public void HorizontalContentLayout()
        {
            NoneContentLayout();
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            layoutGroup = content.AddComponent<HorizontalLayoutGroup>();
            ContentSizeFitter sizefilter = content.AddComponent<ContentSizeFitter>();
            sizefilter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            sizefilter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            layoutGroup = content.GetComponent<LayoutGroup>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);

        }
        public void VerticalContentLayout()
        {
            NoneContentLayout();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            layoutGroup = content.AddComponent<VerticalLayoutGroup>();
            ContentSizeFitter sizefilter = content.AddComponent<ContentSizeFitter>();
            sizefilter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizefilter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }
        public void GridContentLayout()
        {
            NoneContentLayout();
            scrollRect.horizontal = true;
            scrollRect.vertical = true;
            layoutGroup = content.AddComponent<GridLayoutGroup>();
            ContentSizeFitter sizefilter = content.AddComponent<ContentSizeFitter>();
            sizefilter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizefilter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        public void ScrollRectPadding(SimpleListOption option, List<GameObject> gos)
        {
            if (layoutGroup.IsUnityNull())
            {
                if (content.TryGetComponent(out LayoutGroup lay))
                {
                    layoutGroup = lay;
                }
            }
            layoutGroup.padding.left = option.left;
            layoutGroup.padding.right = option.right;
            layoutGroup.padding.top = option.top;
            layoutGroup.padding.bottom = option.bottom;

            switch (option.contentLayoutFilter)
            {
                case SimpleListContentLayoutFilter.None:
                    break;
                case SimpleListContentLayoutFilter.Horizontal:
                    HorizontalLayoutGroup h = (HorizontalLayoutGroup)layoutGroup;
                    ChangeShellSize(option, gos);
                    h.spacing = option.spacing;
                    break;
                case SimpleListContentLayoutFilter.Vertical:
                    VerticalLayoutGroup v = (VerticalLayoutGroup)layoutGroup;
                    ChangeShellSize(option, gos);
                    v.spacing = option.spacing;
                    break;
                case SimpleListContentLayoutFilter.Grid:
                    GridLayoutGroup g = (GridLayoutGroup)layoutGroup;
                    g.cellSize = option.cellSize;
                    g.spacing = option.gridSpacing;
                    break;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        private void ChangeShellSize(SimpleListOption option, List<GameObject> gos)
        {
            for (int i = 0; i < gos.Count; i++)
            {
                RectTransform rt = gos[i].transform.GetComponent<RectTransform>();
                rt.sizeDelta = option.cellSize;
            }
        }
    }

}
