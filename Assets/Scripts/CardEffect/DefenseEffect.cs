using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "CardEffect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (targetType == EffectTargetType.Self)
        {
            from.UpdateDefense(value);
        }
    }
}
