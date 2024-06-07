using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisual : MonoBehaviour
{
    [SerializeField] private Vector2 offset = new Vector2(1, 0);
    [SerializeField] private float curvatureFactor = 1f;
    [SerializeField] private bool randomRotationEffect;

    public void OrganizeHandVisual(List<Card> cards)
    {
        int centerIndex = 0;
        if (cards.Count % 2 == 0)
            centerIndex = Mathf.RoundToInt(cards.Count / 2) - 1;
        else
            centerIndex = Mathf.RoundToInt(cards.Count / 2);

        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.SetParent(transform);
            float posX = i - centerIndex;
            float posY = CalculateParabolicY(posX, curvatureFactor/1000); 

            Vector3 pos = new Vector3(posX + offset.x * posX, posY, -(0.01f * i));
            sequence.Join(cards[i].transform.DOLocalMove(pos, 0.3f));

            if (!randomRotationEffect)
            {
                if (cards.Count > 1)
                {
                    float rotationAngle = Mathf.Lerp(-curvatureFactor, curvatureFactor, (float)i / (cards.Count - 1));
                    cards[i].transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
                }
            }
            else if(randomRotationEffect && i == cards.Count - 1)
            {
                Vector3 randomRotation = new Vector3(0,0,Random.Range(-5f,5f));
                sequence.Join(cards[i].transform.DOLocalRotate(randomRotation, 0.3f));
            }
        }
    }

    private float CalculateParabolicY(float x, float curvature)
    {
        return curvature * x * x;
    }
}
