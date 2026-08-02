using UnityEngine;

// Dice의 고정(픽스) 상태를 시각적으로 표시한다. 머티리얼 에셋을 직접 바꾸지 않도록
// MaterialPropertyBlock을 사용해 이 오브젝트에만 색상을 덧입힌다.
[RequireComponent(typeof(Dice), typeof(Renderer), typeof(AudioSource))]
public class DiceView : MonoBehaviour
{
    [SerializeField] private Color fixedTintColor = new Color(1f, 0.85f, 0.2f); // 고정 시 강조 색상
    [SerializeField] private AudioClip fixToggleSound; // 고정 on/off 시 재생 (공용)

    // URP Lit/Simple Lit 셰이더 기준. 커스텀 셰이더를 쓰면 프로퍼티 이름을 맞춰야 한다.
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private Dice dice;
    private Renderer rend;
    private AudioSource audioSource;
    private MaterialPropertyBlock propertyBlock;
    private Color originalColor;

    void Awake()
    {
        dice = GetComponent<Dice>();
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        propertyBlock = new MaterialPropertyBlock();
        originalColor = rend.sharedMaterial.HasProperty(BaseColor)
            ? rend.sharedMaterial.GetColor(BaseColor)
            : Color.white;
    }

    void OnEnable()
    {
        dice.OnFixedChanged += HandleFixedChanged;
    }

    void OnDisable()
    {
        dice.OnFixedChanged -= HandleFixedChanged;
    }

    void HandleFixedChanged(bool isFixed)
    {
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, isFixed ? fixedTintColor : originalColor);
        rend.SetPropertyBlock(propertyBlock);

        PlayFixToggleSound();
    }

    void PlayFixToggleSound()
    {
        if (fixToggleSound == null) return;
        audioSource.PlayOneShot(fixToggleSound);
    }
}
