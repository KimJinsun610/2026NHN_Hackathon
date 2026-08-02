using UnityEngine;

// 씬 전환 시에도 배경음악이 끊기지 않도록 유지하는 싱글톤.
// 최초 진입 씬에만 이 컴포넌트가 붙은 오브젝트를 하나 두면 된다.
[RequireComponent(typeof(AudioSource))]
public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject); // 씬 재진입 등으로 중복 생성되면 새로 생긴 쪽을 제거
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // 씬 진입 시 SceneBgmTrigger가 호출. 이미 같은 곡이 재생 중이면 끊지 않고 그대로 둔다.
    public void PlayBgm(AudioClip clip, bool loop = true)
    {
        if (clip == null || audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }
}
