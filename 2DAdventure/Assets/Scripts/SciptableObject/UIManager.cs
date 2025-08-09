using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar  playerStatBar;

    [Header("事件监听")]
    public CharacterEventSO healthEvent;
    public SceneLoadEventSO loadEvent;

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        loadEvent.LaodRequestEvent += OnLoadEvent;
    }

    public void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEvent.LaodRequestEvent -= OnLoadEvent;
    }

    private void OnLoadEvent(GameSceneSO sceneToLoad, Vector3 arg1, bool arg2)
    {
        var isMenu = sceneToLoad.sceneType == SceneType.Menu;
        playerStatBar.gameObject.SetActive(!isMenu);
    }

    private void OnHealthEvent(Character character)
    {
        var persentage = character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);

        playerStatBar.OnPowerChange(character);
    }
}
