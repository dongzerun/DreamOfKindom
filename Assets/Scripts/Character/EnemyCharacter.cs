using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCharacter : CharacterBase
{
    public EnemyActionDataSO actionDataSO;
    public EnemyAction currentAction;


    protected PlayerCharacter player;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
    }

    public virtual void OnPlayerTurnBegin()
    {
        // get a random action index
        var idx = Random.Range(0, actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[idx];
    }

    public virtual void OnEnemyTurnBegin()
    {
        switch (currentAction.effect.targetType)
        {
            case EffectTargetType.Self:
                Skill();
                break;
            case EffectTargetType.SingleTarget:
                Attact();
                break;
            case EffectTargetType.ALLTargets:
                break;
            default:
                break;
        }
    }

    public virtual void Attact()
    {
        //animator.SetTrigger("attack");
        //currentAction.effect.Execute(this, player);
        StartCoroutine(ProcessDelayAction("attack"));
    }

    public virtual void Skill()
    {
        //animator.SetTrigger("skill");
        //currentAction.effect.Execute(this, this);
        StartCoroutine(ProcessDelayAction("skill"));
    }

    IEnumerator ProcessDelayAction(string actionName)
    {
        animator.SetTrigger(actionName);

        yield return new WaitUntil(
            () => animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f > 0.6f
            && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName)
            && !animator.IsInTransition(0));

        switch (actionName)
        {
            case "skill":
                currentAction.effect.Execute(this, this);
                break;

            case "attack":
                currentAction.effect.Execute(this, player);
                break;
        }
    }
}
