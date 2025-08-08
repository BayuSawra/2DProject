using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//这是一个unity的SO脚本写法，目前还没有学过。
[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    public UnityAction<Character> OnEventRaised;
    
    public void RaiseEvent(Character character)
    {
       OnEventRaised?.Invoke(character);
    }
}
