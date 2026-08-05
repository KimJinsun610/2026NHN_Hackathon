using UnityEngine;

// BgmManager와 쌍을 이루는 효과음 전담 매니저
[RequireComponent(typeof(AudioSource))]
public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        Instance = this;
        sfxSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // 기본 효과음 재생 
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // 소리 크기를 조절해서 재생하고 싶을 때 사용하는 오버로딩 함수
    public void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }
}