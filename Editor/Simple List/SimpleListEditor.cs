using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using UGUICUSTOM;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace UGUICUSTOM
{
    [CustomEditor(typeof(SimpleList), true)]
    [CanEditMultipleObjects]
    public class SimpleListEditor : Editor
    {
        private SimpleList list;
        // private float horizontalScrollValue;
        // private float verticalScrollValue;
        private bool _showLayoutSettings = true;
        private void OnEnable()
        {
            list = (SimpleList)target;
            list.OnReImport();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            OnDrawingObjectField();
            OnDrawLayoutOptionField();
            serializedObject.ApplyModifiedProperties();
        }
        #region Content Element Control
        private void OnDrawingObjectField()
        {
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("[List Element Options]", BigLabelStyle);
            list.Element = (GameObject)EditorGUILayout.ObjectField("Element", list.Element, typeof(GameObject), true);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create"))
            {
                list.OnCreateElement();
            }
            if (GUILayout.Button("Clear"))
            {
                list.OnClearElements();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            SerializedProperty eventProp = serializedObject.FindProperty("ElementEvent");
            EditorGUILayout.PropertyField(eventProp, new GUIContent("List Element Event"), true);
        }

        #endregion

        #region Layout Option
        private void OnDrawLayoutOptionField()
        {
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("[Content Layout Options]", BigLabelStyle);
            OptionEnumFilter();
            if (list.SimpleListOption.contentLayoutFilter != 0)
                OptionLayoutSetting();
        }
        private void OptionEnumFilter()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.Space(5);
            list.SimpleListOption.contentLayoutFilter = (SimpleListContentLayoutFilter)EditorGUILayout.EnumPopup(list.SimpleListOption.contentLayoutFilter);
            if (EditorGUI.EndChangeCheck())
            {
                list.OnChangedLayoutFilter();
            }
        }
        private void OptionLayoutSetting()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            _showLayoutSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_showLayoutSettings, "Content Layout Setting");
            if (GUILayout.Button("Reset"))
            {
                list.SimpleListOption.top = 0;
                list.SimpleListOption.left = 0;
                list.SimpleListOption.right = 0;
                list.SimpleListOption.bottom = 0;
                list.SimpleListOption.spacing = 0;
                //Change Float Event
                list.OnChangeLayoutOption();
            }
            EditorGUILayout.EndHorizontal();

            // 2. true일 때만 내부 내용 그리기
            if (_showLayoutSettings)
            {
                // 너비 설정을 위한 변수들
                float fieldWidth = 50f;  // 입력창 너비
                float centerSpace = 60f; // 중앙 이미지/공백 너비
                float totalWidth = (fieldWidth * 2) + centerSpace; // 총 너비 (Left + Center + Right)
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                // --- 4방위 레이아웃 (중앙 정렬) ---
                EditorGUILayout.BeginVertical();
                // 1. Top
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                DrawFieldWithLabel("Top", ref list.SimpleListOption.top, fieldWidth);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                // 2. Left - Center - Right
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                // 전체를 감싸는 컨테이너 시작 (이 안의 너비가 totalWidth가 됨)
                EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));
                DrawFieldWithLabel("Left", ref list.SimpleListOption.left, fieldWidth);
                GUILayout.FlexibleSpace();

                // 중앙 이미지 영역
                Rect imgRect = GUILayoutUtility.GetRect(centerSpace, 50);
                GUI.Box(imgRect, "Layout\nGroup", MiniBoldCenteredStyle);
                GUILayout.FlexibleSpace();
                DrawFieldWithLabel("Right", ref list.SimpleListOption.right, fieldWidth);

                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                // 3. Bottom
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                DrawFieldWithLabel("Bottom", ref list.SimpleListOption.bottom, fieldWidth, true);
                GUILayout.FlexibleSpace();

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space(10);

                // --- 4. Spacing (위의 Left~Right 끝선에 맞춤) ---

                // 핵심: 위에서 계산한 totalWidth를 그대로 사용

                if (list.SimpleListOption.contentLayoutFilter.Equals(SimpleListContentLayoutFilter.Grid))
                {
                    list.SimpleListOption.cellSize = EditorGUILayout.Vector2Field("shellSize", list.SimpleListOption.cellSize);

                    list.SimpleListOption.gridSpacing = EditorGUILayout.Vector2Field("Grid Spacing", list.SimpleListOption.gridSpacing);
                }
                else
                {
                    list.SimpleListOption.cellSize = EditorGUILayout.Vector2Field("shellSize", list.SimpleListOption.cellSize);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(totalWidth));
                    EditorGUILayout.LabelField("Spacing", GUILayout.Width(55)); // 라벨 너비 고정
                    list.SimpleListOption.spacing = EditorGUILayout.FloatField(list.SimpleListOption.spacing);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }


                EditorGUILayout.EndVertical();
                // GUILayout.FlexibleSpace();
                // ScrollVerticalSlider();
                // GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                // ScrollHorizontalSlider();
                // GUILayout.Space(10);
                if (EditorGUI.EndChangeCheck())
                {
                    //Change Float Event
                    list.OnChangeLayoutOption();
                }
            }

        }
        void DrawFieldWithLabel(string label, ref int value, float width, bool labelBottom = false)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(width));
            if (!labelBottom) GUILayout.Label(label, EditorStyles.miniLabel);
            value = EditorGUILayout.IntField(value, GUILayout.Width(width));
            if (labelBottom) GUILayout.Label(label, EditorStyles.miniLabel);
            EditorGUILayout.EndVertical();
        }
        // private void ScrollHorizontalSlider()
        // {
        //     EditorGUI.BeginChangeCheck();
        //     horizontalScrollValue = GUILayout.HorizontalSlider(horizontalScrollValue, 1f, 0f);
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         Debug.Log("horizontal");
        //         list.OnChangeHorizontal(horizontalScrollValue);
        //     }

        // }
        // private void ScrollVerticalSlider()
        // {
        //     EditorGUI.BeginChangeCheck();

        //     verticalScrollValue = GUILayout.VerticalSlider(verticalScrollValue, 1f, 0f, GUILayout.Height(150));
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         list.OnChangeVertical(verticalScrollValue);
        //     }

        // }


        private GUIStyle _miniBoldCenteredStyle;
        private GUIStyle MiniBoldCenteredStyle
        {
            get
            {
                if (_miniBoldCenteredStyle == null)
                {
                    // 기존 miniBoldLabel의 모든 설정을 복사
                    _miniBoldCenteredStyle = new GUIStyle(EditorStyles.miniBoldLabel);
                    // 정렬만 중앙으로 변경
                    _miniBoldCenteredStyle.alignment = TextAnchor.MiddleCenter;
                }
                return _miniBoldCenteredStyle;
            }
        }
        private GUIStyle _bigLabelStyle;
        private GUIStyle BigLabelStyle
        {
            get
            {
                if (_bigLabelStyle == null)
                {
                    // 기본 라벨 스타일 복사
                    _bigLabelStyle = new GUIStyle(EditorStyles.label);
                    // 글자 크기 설정 (기본은 보통 12)
                    _bigLabelStyle.fontSize = 15;
                    // 필요하다면 볼드체 적용
                    _bigLabelStyle.fontStyle = FontStyle.Bold;
                    // 텍스트 색상 유지 (또는 변경 가능)
                    _bigLabelStyle.normal.textColor = Color.white;
                }
                return _bigLabelStyle;
            }
        }
        #endregion
    }
}