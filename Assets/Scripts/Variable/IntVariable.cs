using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName ="Variable/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int MaxHp;

    public int CurrentHp;

    public IntEventSO ValueChangedEvent;

    public void SetValue(int value)
    {
        var changed = CurrentHp != value ? true : false;
        CurrentHp = value;
        if (changed) 
        {
            ValueChangedEvent?.RaiseEvent(value, this);
        }
    }
}
