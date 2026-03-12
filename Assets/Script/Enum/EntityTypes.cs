using System;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

namespace GameData.Types
{
    // 1. 엔티티 진영 구분
    public enum EntityType : byte
    {
        None = 0,
        Player,
        Enemy,
        Neutral
    }
    public enum SlotType : byte
    {
        Profile = 0,
        FirstSlot,
        SecondSlot,
        NextSlot,
    }

    // 2. 죄악 속성 (Sin Attributes)
    public enum SinType : byte
    {
        None = 0,
        Wrath,   // 분노
        Lust,    // 색욕
        Sloth,   // 나태
        Glut, // 폭식
        Gloom,   // 우울
        Pride,   // 오만
        Envy     // 질투
    }

    // 3. 공격 판정 타입
    public enum AttackType : byte
    {
        None = 0,
        Blunt, // 타격
        Slash, // 참격
        Pierce // 관통
    }

    // 4. 방어 행동 타입
    public enum DefenseType : byte
    {
        None = 0,
        Evade,   // 회피
        Counter, // 반격
        Guard    // 방어
    }
    // 5. 엔티티 정보
    [System.Serializable]
    public struct EntityEntry
    {
#if UNITY_EDITOR
        public String Editor_ID;
#endif
        public int EntityIDHash;
        public int MaxHp;
        public int MinSpeed;
        public int MaxSpeed;
        public EntityType Type;

#if UNITY_EDITOR
        public String skllID1;
        public String skllID2;
        public String skllID3;
        public String defenseID;
#endif
        public int SkillIndex1;
        public int SkillIndex2;
        public int SkillIndex3;
        public int Defense;
    }
    // 6. 스킬 정보
    [System.Serializable]
    public struct SkillEntry // 개별 스킬 정보 (값 타입)
    {
        #if UNITY_EDITOR
        public String Editor_Owner_ID;
        #endif
        public int Skills_Num;
        public int SkillIDHash;
        public int BaseDamage;
        public int CoinValue;
        public int CoinCount;
        public SinType SinType;
        public AttackType AttackType;
        public DefenseType DefenseType;
        public Sprite skillIcon;
    }


}