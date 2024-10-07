using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite cardImage;
    public int cost;
    public CardType cardType;

    [TextArea]
    public string description;

    // 实际执行效果的列表，可以有多个
    public List<Effect> effects;
}
