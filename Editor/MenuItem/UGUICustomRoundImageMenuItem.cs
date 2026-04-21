
using UGUICUSTOM;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UGUICustomRoundImageMenuItem : MonoBehaviour
{
    [MenuItem("GameObject/UI/Round Image", false)]
    static void MenuItem(MenuCommand menuCommand)
    {
        // 1. Custom GameObject 이름으로 새 Object를 만든다.
        GameObject go = new GameObject("Round Image");
        go.AddComponent<RoundImage>();

        go.transform.SetParent(FindAnyObjectByType<Canvas>().transform, false);

        // 2. Hierachy 윈도우에서 어떤 오브젝트를 선택하여 생성시에는 그 오브젝트의 하위 계층으로 생성된다.
        // 그밖의 경우에는 아무일도 일어나지 않는다.
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        // 3. 생성된 오브젝트를 Undo 시스템에 등록한다.
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

        // 4. 생성한 오브젝트를 선택한다.
        Selection.activeObject = go;
    }

}
