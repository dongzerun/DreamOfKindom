using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "CardEffect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCardEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCardEvent?.RaiseEvent(value, this);
    }
}
