using UnityEngine;

// 씬마다 하나씩 배치. 이 씬에 진입하면 지정된 곡으로 BgmManager를 전환시킨다.
public class SceneBgmTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;

    void Start()
    {
        BgmManager.Instance.PlayBgm(bgmClip);
    }
}
