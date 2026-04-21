using UnityEngine;
namespace UGUICUSTOM
{
    [System.Serializable]
    public class SimpleListOption
    {
        //Layout
        public SimpleListContentLayoutFilter contentLayoutFilter;
        public int top;
        public int left;
        public int right;
        public int bottom;
        public float spacing;
        public Vector2 cellSize;
        public Vector2 gridSpacing;
    }

}
