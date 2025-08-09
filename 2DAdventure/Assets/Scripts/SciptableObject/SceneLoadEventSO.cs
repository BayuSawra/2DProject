using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{

    public UnityAction<GameSceneSO, Vector3, bool> LaodRequestEvent;

    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGO, bool fadeScreen)//场景加载方法，需要location，转场之后人物位置，是否有过度。
    {
        LaodRequestEvent?.Invoke(locationToLoad, posToGO, fadeScreen);
    }

}
