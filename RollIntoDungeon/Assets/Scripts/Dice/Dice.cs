using UnityEngine;
using System;

// 주사위 한 개의 상태(굴리는 중/멈춤)와 고정(픽스) 여부를 관리하는 파사드.
// DiceDrop(굴리기 실행)과 DiceFaceReader(결과 판정)를 감싸서 상위 시스템(DiceManager, DiceSelector)에 노출한다.
[RequireComponent(typeof(DiceDrop), typeof(DiceFaceReader), typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    public enum DiceState
    {
        Idle,
        Rolling,
        Settled
    }

    public DiceState State { get; private set; } = DiceState.Idle;
    public bool IsFixed { get; private set; }
    public DiceFaceReader.DiceFace LastResult { get; private set; }

    public event Action<bool> OnFixedChanged;
    public event Action<DiceState> OnStateChanged;

    private DiceDrop drop;
    private DiceFaceReader faceReader;
    private Rigidbody rb;

    void Awake()
    {
        drop = GetComponent<DiceDrop>();
        faceReader = GetComponent<DiceFaceReader>();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        faceReader.OnSettled += HandleSettled;
    }

    void OnDisable()
    {
        faceReader.OnSettled -= HandleSettled;
    }

    // DiceManager가 호출. 고정된 주사위는 굴리지 않고 스스로 스킵한다.
    public void Roll()
    {
        if (IsFixed) return;

        rb.isKinematic = false;
        SetState(DiceState.Rolling);
        drop.Drop();
    }

    // DiceSelector가 클릭 시 호출. 멈춘 상태(Settled)에서만 고정/해제 가능.
    public void ToggleFix()
    {
        if (State != DiceState.Settled) return;

        IsFixed = !IsFixed;
        rb.isKinematic = IsFixed; // 고정된 주사위는 다른 주사위 리롤에 영향받지 않도록 물리 정지
        OnFixedChanged?.Invoke(IsFixed);
    }

    // 새 라운드 시작 시 DiceManager가 전체 주사위에 대해 호출
    public void ResetFix()
    {
        if (!IsFixed) return;

        IsFixed = false;
        rb.isKinematic = false;
        OnFixedChanged?.Invoke(false);
    }

    void HandleSettled(DiceFaceReader.DiceFace result)
    {
        LastResult = result;
        SetState(DiceState.Settled);
    }

    void SetState(DiceState next)
    {
        if (State == next) return;
        State = next;
        OnStateChanged?.Invoke(next);
    }
}
