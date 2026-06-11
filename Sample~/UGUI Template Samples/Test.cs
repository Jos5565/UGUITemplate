using UGUICUSTOM;
using Unity.Android.Gradle;
using UnityEngine;

public class Test : MonoBehaviour
{
    public SimpleList list;
    public SampleElement element;
    public void CreateSampleList()
    {
        SampleElement ele = list.OnCreateElementSelf<SampleElement>(element);
    }
    public void TestFunc(string test)
    {
        Debug.Log(test);
    }
}
