using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro를 사용하기 위한 네임스페이스

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;        // 체력바 게이지 이미지
    [SerializeField] private TextMeshProUGUI hpText; // 체력 숫자 텍스트

    // 체력이 바뀔 때마다 이 함수를 부를 겁니다.
    public void UpdateHP(int currentHP, int maxHP)
    {
        // 1. 게이지 조절 (fillAmount는 0~1 사이의 비율을 사용합니다)
        fillImage.fillAmount = (float)currentHP / maxHP;

        // 2. 텍스트 업데이트 (예: "80 / 100")
        if (hpText != null)
        {
            hpText.text = $"{currentHP} / {maxHP}";
        }
    }
}