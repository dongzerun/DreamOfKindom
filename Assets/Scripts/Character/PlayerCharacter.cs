using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCharacter : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;
    public int CurrentMana { get => playerMana.CurrentHp; set => playerMana.SetValue(value); }

    private void OnEnable()
    {
        playerMana.MaxHp = maxMana;
        CurrentMana = playerMana.MaxHp;
    }

    public void NewTurn()
    {
        Debug.Log("PlayerCharacter new turn " + maxMana);
        CurrentMana = maxMana;
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana <= 0)
            CurrentMana = 0;
    }

    public void OnNewGameStart(object obj)
    {
        CurrentHP = MaxHP;
        isDead = false;
        CurrentMana = maxMana;
        NewTurn();
    }
}
