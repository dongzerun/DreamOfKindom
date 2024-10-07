using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private VisualElement cardContainer;

    public CardManager cardManager;
    public VisualTreeAsset cardTemplate;

    private CardDataSO currentCardData;
    private List<Button> cardButtons = new();
    private Button confirmButton;

    public ObjectEventSO finishPickCardEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("Confirm");
        confirmButton.clicked += OnConfirmButtonClicked;

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            InitCard(card, data);
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);

            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton, data);
        }
    }

    private void OnConfirmButtonClicked()
    {
        cardManager.UnLockCard(currentCardData);
        finishPickCardEvent.RaiseEvent(null, this);
    }

    private void OnCardClicked(Button cardButton, CardDataSO data)
    {
        currentCardData = data;
        Debug.Log("Card clicked: "+ currentCardData.cardName);
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if (cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else 
            {
                cardButtons[i].SetEnabled(true);
            }
        }
    }

    public void InitCard(VisualElement card, CardDataSO cardData) 
    {
        // binding card data
        card.dataSource = cardData;

        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");
        var cardName = card.Q<Label>("CardName");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "¹¥»÷",
            CardType.Defense => "·ÀÓù",
            CardType.Abilities => "ÄÜÁ¦",
            CardType.Heal => "ÖÎÁÆ",
            _ => throw new System.NotImplementedException(),
        };
    }
}
