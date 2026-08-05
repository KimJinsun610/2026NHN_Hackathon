using UnityEngine;

// Lobby 씬의 공용 UI 클릭 사운드 재생기.
// 등록하기 버튼 / 보유 주사위 선택은 같은 사운드, 들고갈 주사위 해제는 다른 사운드를 쓴다.
[RequireComponent(typeof(AudioSource))]
public class LobbyAudio : MonoBehaviour
{
    [SerializeField] private AudioClip selectSound;  // 등록하기 + 보유 주사위 선택 공용
    [SerializeField] private AudioClip unequipSound; // 들고갈 주사위 해제 전용

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySelectSound() => Play(selectSound);
    public void PlayUnequipSound() => Play(unequipSound);

    // 사운드 재생 직후 씬을 전환하는 곳(NextRoundButton 등)이 전환을 얼마나 늦춰야 할지 알기 위해 사용
    public float SelectSoundLength => selectSound != null ? selectSound.length : 0f;

    void Play(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }
}
