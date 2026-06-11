using System;
using System.Collections.Generic;
using System.Linq;
using UGUICUSTOM;
using UnityEngine;
using UnityEngine.UI;

public static class ToggleGroupExtensions
{
    public static List<ButtonToggle> m_Toggles = new List<ButtonToggle>();


    public static void UnregisterToggle(this ToggleGroup toggleGroup, ButtonToggle toggle)
    {
        if (m_Toggles.Contains(toggle))
        {
            m_Toggles.Remove(toggle);
        }
    }
    public static void RegisterToggle(this ToggleGroup toggleGroup, ButtonToggle toggle)
    {
        if (!m_Toggles.Contains(toggle))
        {
            m_Toggles.Add(toggle);
        }
    }
    public static void NotifyToggleOn(this ToggleGroup toggleGroup, ButtonToggle toggle, bool sendCallback = true)
    {
        ValidateToggleIsInGroup(toggle);
        for (int i = 0; i < m_Toggles.Count; i++)
        {
            if (!(m_Toggles[i] == toggle))
            {
                if (sendCallback)
                {
                    m_Toggles[i].isOn = false;
                }
                else
                {
                    m_Toggles[i].SetIsOnWithoutNotify(value: false);
                }
            }
        }
    }
    private static void ValidateToggleIsInGroup(ButtonToggle toggle)
    {
        if (toggle == null || !m_Toggles.Contains(toggle))
        {
            throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}"));
        }
    }
    public static void NotifyToggleOn(ButtonToggle toggle, bool sendCallback = true)
    {
        ValidateToggleIsInGroup(toggle);
        for (int i = 0; i < m_Toggles.Count; i++)
        {
            if (!(m_Toggles[i] == toggle))
            {
                if (sendCallback)
                {
                    m_Toggles[i].isOn = false;
                }
                else
                {
                    m_Toggles[i].SetIsOnWithoutNotify(value: false);
                }
            }
        }
    }
    public static bool AnyButtonTogglesOn(this ToggleGroup toggleGroup)
    {
        return m_Toggles.Find((ButtonToggle x) => x.isOn) != null;
    }
    // public static void EnsureValidState(this ToggleGroup toggleGroup)
    // {
    //     Debug.Log(toggleGroup.gameObject.name);
    //     if (!toggleGroup.allowSwitchOff && !AnyButtonTogglesOn(toggleGroup) && m_Toggles.Count != 0)
    //     {
    //         m_Toggles[0].isOn = true;
    //         NotifyToggleOn(m_Toggles[0]);
    //     }

    //     IEnumerable<ButtonToggle> enumerable = ActiveToggles(toggleGroup);
    //     if (enumerable.Count() <= 1)
    //     {
    //         return;
    //     }

    //     ButtonToggle firstActiveToggle = GetFirstActiveToggle(toggleGroup);
    //     foreach (ButtonToggle item in enumerable)
    //     {
    //         if (!(item == firstActiveToggle))
    //         {
    //             item.isOn = false;
    //         }
    //     }
    // }

    // public static IEnumerable<ButtonToggle> ActiveToggles(this ToggleGroup toggleGroup)
    // {
    //     return m_Toggles.Where((ButtonToggle x) => x.isOn);
    // }

    // public static ButtonToggle GetFirstActiveToggle(this ToggleGroup toggleGroup)
    // {
    //     IEnumerable<ButtonToggle> source = ActiveToggles(toggleGroup);
    //     if (source.Count() <= 0)
    //     {
    //         return null;
    //     }

    //     return source.First();
    // }


}
