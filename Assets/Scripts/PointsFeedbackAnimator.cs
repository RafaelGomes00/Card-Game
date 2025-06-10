using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsFeedbackAnimator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI feedbackText;
    public void SetText(int receivedPoints, int combo)
    {
        feedbackText.text = $"{(receivedPoints > 0? "+" : "") + receivedPoints}" + (combo > 1 ? $" ({combo}x Combo)" : "");
    }

    public void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
