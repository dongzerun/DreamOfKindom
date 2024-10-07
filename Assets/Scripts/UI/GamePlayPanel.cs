using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
    public ObjectEventSO playerEndEvent;

    private VisualElement rootElement;
    private Label energyAmount, drawAmount, discardAmount, turnLabel;
    private Button endTurnButton;



    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        
        energyAmount = rootElement.Q<Label>("EnergyAmount");
        drawAmount = rootElement.Q<Label>("DrawAmount");
        discardAmount = rootElement.Q<Label>("DiscardAmount");

        turnLabel = rootElement.Q<Label>("TurnLabel");
        endTurnButton = rootElement.Q<Button>("EndTurn");
        endTurnButton.clicked += OnEndPlayerTurn;


        energyAmount.text = "0";
        drawAmount.text = "6";
        discardAmount.text = "0";
        turnLabel.text = "��Ϸ��ʼ";
    }

    private void OnEndPlayerTurn()
    {
        playerEndEvent.RaiseEvent(null, this);
    }

    public void UpdateDrawCountAmount(int amount)
    {
        drawAmount.text = amount.ToString();
    }

    public void UpdateDiscardCountAmount(int amount)
    {
        discardAmount.text = amount.ToString();
    }

    public void UpdateEnergyCountAmount(int amount)
    {
        energyAmount.text = amount.ToString();
    }

    public void OnEnemyTurnBegin()
    {
        Debug.Log("GamePlayPanel: enemy turn begin");
        endTurnButton.SetEnabled(false);
        turnLabel.text = "�з��غ�";
        turnLabel.style.color = new StyleColor(Color.red);
    }

    public void OnPlayerTurnBegin()
    {
        Debug.Log("GamePlayPanel: player turn begin");
        endTurnButton.SetEnabled(true);
        turnLabel.text = "��һغ�";
        turnLabel.style.color = new StyleColor(Color.white);
    }
}
