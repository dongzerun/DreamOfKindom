using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "CardEffect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch (targetType)
        {
            case EffectTargetType.Self:
                from.SetupStrength(value, true);
                break;

            case EffectTargetType.SingleTarget:
                target.SetupStrength(value, false);
                break;

            case EffectTargetType.ALLTargets:

                break;
            default:
                break;
        }
    }
}
