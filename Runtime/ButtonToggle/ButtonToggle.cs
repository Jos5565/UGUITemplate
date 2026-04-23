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
        public bool isOn = false;
        private Sprite toggleDefaultSprite;
        private Color toggleDefaultColor;
        public Color toggleCheckColor;
        public Sprite toggleCheckSprite;
        private Image image;
        protected ButtonToggle()
        { }

        protected override void OnEnable()
        {
            base.OnEnable();
            Initialize();
        }
        public void Initialize()
        {
            if (!transform.TryGetComponent(out Graphic g))
            {
                image = transform.AddComponent<Image>();
                targetGraphic = image;
            }
            else
            {
                image = (Image)g;
            }
            if (!image.sprite.IsUnityNull()) toggleDefaultSprite = image.sprite;
            else if (image.sprite.IsUnityNull()) toggleDefaultColor = image.color;
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
                //image.sprite = isOn ? toggleCheckSprite : toggleDefaultSprite;
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
            if (!image.sprite.IsUnityNull() && isOn)
            {
                image.sprite = toggleCheckSprite;
            }
            else if (!image.sprite.IsUnityNull() && !isOn)
            {
                image.sprite = toggleDefaultSprite;
            }
            else if (image.sprite.IsUnityNull() && isOn)
            {
                image.color = toggleCheckColor;
            }
            else if (image.sprite.IsUnityNull() && !isOn)
            {
                image.color = toggleDefaultColor;
            }
        }
    }

}

