using UnityEngine;

// 주사위가 구를 때 바닥/다른 주사위와 부딪힐 때마다 랜덤 클랙 사운드를 재생한다.
[RequireComponent(typeof(Dice), typeof(AudioSource))]
public class DiceAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] impactClips;
    [SerializeField] private float minImpactSpeed = 0.5f; // 이 속도 미만 충돌은 무시 (미세한 떨림 소음 방지)
    [SerializeField] private float maxImpactSpeed = 6f;    // 이 속도 이상이면 최대 볼륨
    [SerializeField, Range(0f, 1f)] private float minVolume = 0.5f; // 약한 충돌이어도 최소 이 크기는 보장
    [SerializeField, Range(0f, 1f)] private float maxVolume = 1f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    private Dice dice;
    private AudioSource audioSource;

    void Awake()
    {
        dice = GetComponent<Dice>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D 사운드 — 주사위 위치에서 소리가 나도록
    }

    void OnCollisionEnter(Collision collision)
    {
        if (dice.State != Dice.DiceState.Rolling) return; // 멈춘/고정된 주사위가 스쳐도 소리 안 나게
        if (impactClips == null || impactClips.Length == 0) return;

        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed < minImpactSpeed) return;

        float t = Mathf.InverseLerp(minImpactSpeed, maxImpactSpeed, impactSpeed);
        float volume = Mathf.Lerp(minVolume, maxVolume, t);
        AudioClip clip = impactClips[Random.Range(0, impactClips.Length)];

        audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
        audioSource.PlayOneShot(clip, volume); // 겹쳐서 재생돼도 서로 끊기지 않음
    }
}
