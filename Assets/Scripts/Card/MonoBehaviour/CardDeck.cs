using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager cardLayoutManager;
    public Vector3 deckPosition; // ³éÅÆ¶ÑÎ»ÖÃ

    public List<CardDataSO> drawDeck = new(); // ³éÅÆ¿â
    public List<CardDataSO> discardDeck = new(); // ÆúÅÆ¶Ñ
    private List<Card> cardObjectAtHandList = new(); // ÊÖÅÆ

    public IntEventSO discardEvent;
    public IntEventSO drawCardEvent;


    private void Start()
    {
        // test, need delete when ready
        InitializeDeck();
    }

    public void InitializeDeck()
    { 
        drawDeck.Clear();

        foreach (var entry in cardManager.currentLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }

        // shuffle cards
        ShuffleDeck();
        discardEvent.RaiseEvent(discardDeck.Count, this);
        drawCardEvent.RaiseEvent(drawDeck.Count, this);
    }

    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }

    public void DrawCardsOnNewTurn(object obj)
    {
        DrawCard(4);
    }
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeck.Count == 0)
            {
                // shuffle
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            CardDataSO currentCardDataSO = drawDeck[0];
            drawDeck.RemoveAt(0);

            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardDataSO);
            card.transform.position = deckPosition;

            cardObjectAtHandList.Add(card);
            var delay = i * 0.05f;
            SetCardPosition(delay);
        }
        discardEvent.RaiseEvent(discardDeck.Count, this);
        drawCardEvent.RaiseEvent(drawDeck.Count, this);
    }

    public void SetCardPosition(float delay)
    {
        for (int i = 0; i < cardObjectAtHandList.Count; i++)
        { 
            Card currentCard = cardObjectAtHandList[i];
            currentCard.isAnimating = true;
            CardTransform cardTransform = cardLayoutManager.GetCardTransofmr(i, cardObjectAtHandList.Count);

            //currentCard.transform.position = cardTransform.pos;
            //var cost = currentCard.cardData.cost;
            //currentCard.isAvailable = cost <= player.CurrentMana;
            currentCard.UpdateCardState();

            var toScale = new Vector3(cardTransform.rotation.x, cardTransform.rotation.y, cardTransform.rotation.z);
            currentCard.transform.DOScale(Vector3.one, 0.1f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.2f);
                currentCard.transform.DOMove(cardTransform.pos, 0.2f).onComplete = () => 
                {
                    currentCard.isAnimating = false;
                };
            };
           
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePosAndRotation(cardTransform.pos, cardTransform.rotation);
        }
    }

    private void ShuffleDeck()
    { 
        discardDeck.Clear();
        // todo
        // ¸üÐÂ UI ÆúÅÆºÍ³éÅÆ¶ÑÊýÁ¿

        for (int i = 0; i < drawDeck.Count; i++)
        { 
            CardDataSO tmp = drawDeck[i];
            int randomIndex = Random.Range(0, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = tmp;
        }
    }

    public void DiscardCard(object obj)
    {
        Card card = obj as Card;

        discardDeck.Add(card.cardData);
        cardObjectAtHandList.Remove(card);

        cardManager.DiscardCard(card.gameObject);

        // ÖØÐÂÅÅÐòÊÖÅÆ
        SetCardPosition(0f);

        discardEvent.RaiseEvent(discardDeck.Count, this);
        drawCardEvent.RaiseEvent(drawDeck.Count, this);
    }

    public void OnPlayerTurnEnd()
    {
        for (int i = 0; i < cardObjectAtHandList.Count; i++)
        {
            discardDeck.Add(cardObjectAtHandList[i].cardData);
            cardManager.DiscardCard(cardObjectAtHandList[i].gameObject);
        }
        cardObjectAtHandList.Clear();
        discardEvent.RaiseEvent(discardDeck.Count, this);
    }

    public void ReleaseAllCards(object obj)
    {
        Debug.Log("Receive release all cards event notify");
        foreach (var card in cardObjectAtHandList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        cardObjectAtHandList.Clear();
        InitializeDeck();
    }
}
