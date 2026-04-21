using System;
using UnityEngine;
using UnityEngine.Events;

public interface ISimpleList
{
    public void AddElement();
}
[Serializable]
public class ElementEvent : UnityEvent { }
