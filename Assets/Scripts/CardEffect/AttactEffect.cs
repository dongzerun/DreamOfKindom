using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "AttactEffect", menuName = "CardEffect/AttactEffect")]
public class AttactEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        if (target == null)
            return;

        switch (targetType) 
        {
            case EffectTargetType.SingleTarget:
                // 攻击乘以力量加成
                var realDamage = (int) math.round(from.baseStrength * value);
                target.TakeDamage(realDamage);
                Debug.Log("Take Damage is: "+ value);
                break;

            case EffectTargetType.ALLTargets:
                // todo:
                // find all enemies within distant and attact them
                foreach (var enemy in GameObject.FindGameObjectsWithTag("enemy"))
                { 
                    enemy.GetComponent<CharacterBase>()?.TakeDamage(value);
                }
                break;
        }
    }
}
