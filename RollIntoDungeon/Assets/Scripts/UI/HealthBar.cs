using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; 

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI hpText;
    
    [Header("Animation Settings")]
    [SerializeField] private float lerpSpeed = 5f; 

    private Coroutine hpCoroutine; 

    public void UpdateHP(int currentHP, int maxHP)
    {
        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}";
        }

        // 목표로 하는 게이지 비율을 계산합니다. (0 ~ 1 사이의 값)
        float targetFill = (float)currentHP / maxHP;

        if (hpCoroutine != null)
        {
            StopCoroutine(hpCoroutine);
        }

        hpCoroutine = StartCoroutine(LerpHP(targetFill));
    }

    private IEnumerator LerpHP(float targetFill)
    {
        while (Mathf.Abs(fillImage.fillAmount - targetFill) > 0.001f)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * lerpSpeed);
            yield return null; 
        }
        
        fillImage.fillAmount = targetFill;
    }
}