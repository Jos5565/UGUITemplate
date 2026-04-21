using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A standard button that sends an event when clicked.
/// </summary>

public class ButtonToggle : Selectable, IPointerClickHandler, ISubmitHandler
{
    public UnityEvent OnClick;
    public UnityEvent<bool> IsOn;
    public bool isToggle = false;
    public bool isOn = false;
    private Sprite toggleDefaultSprite;
    public Sprite toggleCheckSprite;
    private Image image;
    protected ButtonToggle()
    { }

    protected override void OnEnable()
    {
        base.OnEnable();
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
            image.sprite = isOn ? toggleCheckSprite : toggleDefaultSprite;
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
}