using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;


    public IntVariable hp;
    public IntVariable defense;
    public IntVariable strengthBuffRound;

    public int MaxHP { get => hp.MaxHp; }

    public int CurrentHP { get => hp.CurrentHp; set => hp.SetValue(value); }

    protected Animator animator;

    public bool isDead;

    public GameObject buff;
    public GameObject debuff;

    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;

    public ObjectEventSO characterDeadEvent;

    private void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    protected virtual void Awake()
    { 
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.MaxHp = maxHp;
        CurrentHP = MaxHP * 2 / 3;
        strengthBuffRound.SetValue(0);
        ResetDefense();
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) 
            return;

        var currentDamage = (damage - defense.CurrentHp) >= 0 ? damage - defense.CurrentHp : 0;
        var currentDefense = (damage - defense.CurrentHp) >= 0 ? 0: defense.CurrentHp - damage;
        defense.SetValue(currentDefense);

        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            Debug.Log("Current  HP: " + CurrentHP);
            animator.SetTrigger("hit");
        }
        else
        {
            CurrentHP = 0;

            isDead = true;
            // todo:
            // 当前人物或敌人死亡逻辑
            Debug.Log("Get Damage  HP: " + currentDamage);
            characterDeadEvent?.RaiseEvent(this, this);
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.CurrentHp + amount;
        defense.SetValue(value);
    }

    public void ResetDefense()
    { 
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            buff.SetActive(true);
        }
        else
        { 
            debuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = strengthBuffRound.CurrentHp + round;

        if (baseStrength == 1)
        {
            strengthBuffRound.SetValue(0);
        }
        else
        {
            strengthBuffRound.SetValue(currentRound);
        }
    }

    public void UpdateStrengthRound()
    {
        var currentRound = Mathf.Max(strengthBuffRound.CurrentHp -1, 0);
        strengthBuffRound.SetValue(currentRound);
        if (currentRound == 0)
        {
            baseStrength = 1f;
        }
    }
}
