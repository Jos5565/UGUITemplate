using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UGUICUSTOM
{
    public class ButtonToggle : Selectable, IPointerClickHandler, ISubmitHandler
    {
        public UnityEvent OnClick;
        public UnityEvent<bool> IsOn;
        public bool isToggle = false;
        public Sprite toggleDefaultSprite;
        private Color toggleDefaultColor;
        public Color toggleCheckColor;
        public Sprite toggleCheckSprite;
        private Image thisImage;
        private RoundImage roundImage;
        public bool isRound;

        public bool m_IsOn;

        // 외부 코드나 런타임에서 접근할 프로퍼티
        public bool isOn
        {
            get => m_IsOn;
            set
            {
                // 값이 실제로 바뀔 때만 실행
                if (m_IsOn != value)
                {
                    m_IsOn = value;
                    ToggleGrapic(m_IsOn); // 여기서 변경 리스너 실행!
                }
            }
        }
        private bool lastIsOn; // 이전 값을 기억할 변수

        protected ButtonToggle()
        { }

        protected override void OnEnable()
        {
            base.OnEnable();
            Initialize();
        }
        protected override void OnValidate()
        {
            if (isOn != lastIsOn)
            {
                lastIsOn = isOn;
                ToggleGrapic(isOn);
            }
        }
        public void Initialize()
        {
            if (!transform.TryGetComponent(out Graphic g))
            {
                thisImage = transform.AddComponent<Image>();
                targetGraphic = thisImage;
                GetImageGrapics();
                isRound = false;
            }
            else
            {
                if (g is Image)
                {
                    thisImage = (Image)g;
                    GetImageGrapics();
                }
                else if (g is RoundImage)
                {
                    roundImage = (RoundImage)g;
                    GetRoundImageGrapics();
                    isRound = true;
                }
            }

        }

        private void GetImageGrapics()
        {
            isRound = false;
            if (!thisImage.sprite.IsUnityNull())
            {
                toggleDefaultSprite = thisImage.sprite;
            }
            else if (thisImage.sprite.IsUnityNull())
            {
                toggleDefaultColor = thisImage.color;
            }
        }
        private void GetRoundImageGrapics()
        {
            isRound = true;
            if (!roundImage.sprite.IsUnityNull())
            {
                toggleDefaultSprite = roundImage.sprite;
            }
            else if (roundImage.sprite.IsUnityNull())
            {
                toggleDefaultColor = roundImage.color;
            }
        }
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            //Active Event
            if (isToggle)
            {
                isOn = !isOn;
                IsOn?.Invoke(isOn);
                ToggleGrapic(isOn);
                //thisImage.sprite = isOn ? toggleCheckSprite : toggleDefaultSprite;
            }
            else
            {
                OnClick?.Invoke();
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        private void ToggleGrapic(bool isOn)
        {
            if (isRound)
            {
                if (roundImage.sprite == null)
                {
                    roundImage.color = isOn ? toggleCheckColor : toggleDefaultColor;
                }
                if (roundImage.sprite != null)
                {
                    roundImage.sprite = isOn ? toggleCheckSprite : toggleDefaultSprite;
                }
            }
            else
            {
                if (thisImage.sprite == null)
                {
                    thisImage.color = isOn ? toggleCheckColor : toggleDefaultColor;
                }
                if (thisImage.sprite != null)
                {
                    thisImage.sprite = isOn ? toggleCheckSprite : toggleDefaultSprite;
                }
            }


        }
    }

}

