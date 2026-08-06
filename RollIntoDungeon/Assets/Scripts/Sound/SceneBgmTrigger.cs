using UnityEngine;

// 씬마다 하나씩 배치. 이 씬에 진입하면 지정된 곡으로 BgmManager를 전환시킨다.
public class SceneBgmTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip bgmClip;
    [SerializeField, Range(0f, 1f)] private float bgmVolume = 1f; // 씬마다 다른 볼륨을 주고 싶을 때 조절

    void Start()
    {
        BgmManager.Instance.SetVolume(bgmVolume); // 이전 씬(MainScene 등)에서 페이드아웃한 볼륨이 남아있지 않도록 씬 진입 시 항상 지정 볼륨으로 복귀
        BgmManager.Instance.PlayBgm(bgmClip);
    }
}
