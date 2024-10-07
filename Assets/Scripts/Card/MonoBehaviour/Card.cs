using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SpriteRenderer cardSprite;
    public TextMeshPro costText, descrptionText, typeText;
    public CardDataSO cardData;

    // original data
    public Vector3 originalPos;
    public Quaternion originalQua;
    public int originalLayerOrder;

    public bool isAnimating;
    public bool isAvailable; // 是否可以打出

    public PlayerCharacter player;

    public ObjectEventSO discardEvent;
    public IntEventSO costEvent;

    private void Start()
    {

    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descrptionText.text = data.description;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            CardType.Heal => "治疗",
            _ => throw new System.NotImplementedException(),
        };
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }

    public void UpdatePosAndRotation(Vector3 pos, Quaternion qua)
    {
        originalPos = pos;
        originalQua = qua;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isAnimating)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, -3.5f, transform.position.z);
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = 100;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isAnimating)
        {
            return;
        }

        transform.position = originalPos;
        transform.rotation = originalQua;
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ResetCardTransform()
    {
       transform.SetPositionAndRotation(originalPos, originalQua);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteCardEffects(CharacterBase from, CharacterBase target)
    {
        // todo:
        // 1. 减少对应能量 广播事件
        costEvent?.RaiseEvent(cardData.cost, this);


        // 通知回收卡牌
        discardEvent?.RaiseEvent(this, this);
        
        foreach (var effect in cardData.effects)
        {
            effect.Execute(from, target);
        }
    }

    public void UpdateCardState()
    {
        isAvailable = cardData.cost <= player.CurrentMana;
        costText.color = isAvailable ? Color.green:Color.red;
    }
}
