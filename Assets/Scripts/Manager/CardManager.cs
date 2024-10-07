using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;


public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    public CardLibrarySO newGameLibrary;
    public CardLibrarySO currentLibrary;

    private int previsousIndex;

    private void Awake()
    {
        InitializeCardDataList();
        
        foreach (var item in newGameLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCompleted;
    }

    private void OnCompleted(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!!!");
        }
    }

    public GameObject GetCardObject()
    {
        var card =  poolTool.GetObjectFromPool();
        card.transform.localScale = Vector3.zero;
        return card;
    }

    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do
        {
            randomIndex = Random.Range(0, cardDataList.Count);
        } while (previsousIndex == randomIndex);
        previsousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    public void UnLockCard(CardDataSO newCardData)
    {
        var newCard = new CardLibraryEntry
        {
            cardData = newCardData,
            amount = 1,
        };

        if (currentLibrary.cardLibraryList.Contains(newCard))
        {
            Debug.Log("cardLibraryList contain: " + newCardData.name);
            var target = currentLibrary.cardLibraryList.Find(t => t.cardData == newCardData);
            target.amount++;
        }
        else
        { 
            currentLibrary.cardLibraryList.Add(newCard);
        }
    }
}