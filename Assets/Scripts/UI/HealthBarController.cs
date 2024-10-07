using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarController : MonoBehaviour
{
    private CharacterBase currentCharacter;

    public Transform healthBarTransform;

    private UIDocument healthBarDocument;

    private ProgressBar healthBar;

    private VisualElement defenseElement;
    private Label defenseAmount;

    private VisualElement strengthElement;
    public Sprite positiveStrengthSprite;
    public Sprite negtiveStrengthSprite;
    private Label strengthRoundAmount;

    public EnemyCharacter enemy;
    private VisualElement intentElement;
    private Label intentAmount;

    private void Awake()
    {
        currentCharacter = GetComponent<CharacterBase>();
    }

    private void OnEnable()
    {
        InitHealthBar();
    }

    private void MoveToWorldPosition(VisualElement element, Vector3 worldPos, Vector2 size)
    {
        Debug.Log("new pos is " + worldPos);
        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPos, size, Camera.main);
        Debug.Log("original pos is " + element.transform.position);
        element.transform.position = rect.position;
    }


    [ContextMenu("Get UI Document")]
    public void InitHealthBar()
    {
        healthBarDocument = GetComponent<UIDocument>();
        Debug.Log("HEALTH BAR ui document null ? " + healthBarDocument == null);
        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");
        Debug.Log("HEALTH BAR progress bar null ? " + healthBar == null);
        MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);

        defenseElement = healthBar.Q<VisualElement>("Defense");
        defenseAmount = defenseElement.Q<Label>("DefenseAmount");
        // 护甲为0的时候不显示图标
        defenseElement.style.display = DisplayStyle.None;

        strengthElement = healthBar.Q<VisualElement>("StrengthRound");
        strengthRoundAmount = strengthElement.Q<Label>("RoundAmount");
        // 初始化不显示力量加成
        strengthElement.style.display = DisplayStyle.None;

        intentElement = healthBar.Q<VisualElement>("Intent");
        intentAmount = intentElement.Q<Label>("IntentAmount");
        // 初始化不显示意图
        intentElement.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        // todo:
        // 删除，不在这里执行
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (currentCharacter.isDead)
        {
            healthBar.style.display = DisplayStyle.None;
        }

        if (healthBar != null)
        {
            healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";
            healthBar.value = currentCharacter.CurrentHP * 100 / currentCharacter.MaxHP;

            healthBar.RemoveFromClassList("highHealth");
            healthBar.RemoveFromClassList("mediumHealth");
            healthBar.RemoveFromClassList("lowHealth");

            if (healthBar.value < 30f)
            {
                healthBar.AddToClassList("lowHealth");
            }
            else if (healthBar.value < 60)
            {
                healthBar.AddToClassList("mediumHealth");    
            } 
            else 
            {
                healthBar.AddToClassList("highHealth");
            }

            defenseElement.style.display = currentCharacter.defense.CurrentHp > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            defenseAmount.text = currentCharacter.defense.CurrentHp.ToString();
        
            strengthElement.style.display = currentCharacter.strengthBuffRound.CurrentHp > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            strengthRoundAmount.text = currentCharacter.strengthBuffRound.CurrentHp.ToString();
            if (currentCharacter.baseStrength >= 1)
            {
                strengthElement.style.backgroundImage = new StyleBackground(positiveStrengthSprite);
            }
            else
            {
                strengthElement.style.backgroundImage = new StyleBackground(negtiveStrengthSprite);
            }
        }
    }

    public void UpdateIntent()
    {
        intentElement.style.display = DisplayStyle.Flex;
        intentElement.style.backgroundImage = new StyleBackground(enemy.currentAction.actionSprite);
        //intentAmount.text = enemy.currentAction.effect.

        var value = enemy.currentAction.effect.value;
        if (enemy.currentAction.effect.GetType() == typeof(AttactEffect))
        {
            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);
        }
        intentAmount.text = value.ToString();
    }

    public void HideIntent()
    {
        intentElement.style.display = DisplayStyle.None;
    }
}
