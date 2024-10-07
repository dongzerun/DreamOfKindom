using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;

    public float maxWidth = 7f;

    public float cardSpacing = 2f;

    public Vector3 centerPoint;

    // 扇形相关参数
    public float angleBetweenCards = 7f;
    public float radius = 17f;

    [SerializeField]
    private List<Vector3> cardPositions = new();

    private List<Quaternion> cardRotations = new();

    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransofmr(int index, int total)
    {
        CalculatePosition(total, isHorizontal);
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }

    private void CalculatePosition(int cards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();

        if (horizontal)
        {
            float currentWidth = cardSpacing * (cards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (cards - 1) : 0;

            for (int i = 0; i < cards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector3(xPos, centerPoint.y, 0f);

                var rotation = Quaternion.identity;

                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float cardAnagle = (cards - 1) * angleBetweenCards/2;

            for (int i = 0; i < cards; i++)
            {
                var pos = FanCardPosition(cardAnagle - i * angleBetweenCards);
                var ratation = Quaternion.Euler(0,0, cardAnagle - i * angleBetweenCards);
                cardPositions.Add(pos);
                cardRotations.Add(ratation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
            );
    }
}
