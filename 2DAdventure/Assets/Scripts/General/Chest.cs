using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;
    public int healAmount = 10;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openSprite : closeSprite;//箱子被打开了吗？没打开一个图，打开了一个图，用的三元运算符。
    }
    public void TriggerAction()
    {
        Debug.Log("宝箱打开咯！");
        if (!isDone)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";

        // 给予玩家加血（最小改动方案）
        Character character = null;
        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerGO.TryGetComponent<Character>(out character);
        }
        if (character == null)
        {
            // 兜底：场景里找一个带有 PlayerController 的对象并取其 Character
            var playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                character = playerController.GetComponent<Character>();
            }
        }
        if (character != null)
        {
            character.currentHealth = Mathf.Min(character.currentHealth + healAmount, character.maxHealth);
            character.OnHealthChange?.Invoke(character);
        }
        else
        {
            Debug.LogWarning("未找到玩家的 Character 组件，无法执行加血。");
        }
    }
  
}
