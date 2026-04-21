using System;
using UnityEngine;
using UnityEngine.Events;

namespace UGUICUSTOM
{
    public interface ISimpleList
    {
        public void AddElement();
    }
    [Serializable]
    public class ElementEvent : UnityEvent { }

}
