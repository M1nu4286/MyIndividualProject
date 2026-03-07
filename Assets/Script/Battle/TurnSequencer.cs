using UnityEngine;

public class TurnSequencer
{
    // 턴 시작 시 슬롯들을 시각적으로 나열 (연출용)
    public void SequenceSkillSlots(RuntimeEntity[] players, RuntimeEntity[] enemies)
    {
        // 1. 기존 슬롯 프리팹들을 오브젝트 풀에서 가져오거나 활성화
        // 2. players와 enemies의 개수에 맞춰 화면상 위치(Position) 계산
        // 3. 슬롯들을 '스르륵' 나타나게 하는 애니메이션 실행
        Debug.Log("시퀀서: 모든 스킬 슬롯 시각적 배치 완료.");
    }
}