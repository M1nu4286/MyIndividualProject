using GameData.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class CsvImporter
{
    private const string SKILL_CSV = "Assets/Data/SkillDatabase.csv";
    private const string ENTITY_CSV = "Assets/Data/EntityDatabase.csv";

    private const string SKILL_SO = "Assets/Data/SkillDatabase.asset";
    private const string ENTITY_SO = "Assets/Data/EntityDatabase.asset";

    [MenuItem("Tools/Sync All Databases")]
    public static void SyncAll()
    {
        // 1. 스킬 임포트 (지도 생성)
        var skillMap = ImportSkills();

        // 2. 엔티티 임포트 (스킬 인덱스 연결)
        ImportEntities(skillMap);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("모든 데이터 동기화 완료.");
    }

    private static Dictionary<int, int> ImportSkills()
    {
        var db = GetOrCreateSO<SkillDatabase>(SKILL_SO);
        List<SkillEntry> list = new List<SkillEntry>();

        var map = new Dictionary<int, int>();

        using (var reader = new StreamReader(SKILL_CSV))
        {
            reader.ReadLine(); // 헤더 스킵
            int currentIndex = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] d = line.Split(',');
                SkillEntry s = new SkillEntry();
#if UNITY_EDITOR
                s.Editor_Owner_ID = d[0].Trim(); // 에디터에서만 보이는 이름
#endif
                s.SkillIDHash = HashUtility.HashMachine(d[0]);
                s.Skills_Num = int.Parse(d[0].Trim().Substring(d[0].Length-2,2));
                s.BaseDamage = int.Parse(d[1]);
                s.CoinValue = int.Parse(d[2]);
                s.CoinCount = int.Parse(d[3]);

                s.SinType = (SinType)Enum.Parse(typeof(SinType), d[4].Trim(), true);
                s.AttackType = (AttackType)Enum.Parse(typeof(AttackType), d[5].Trim(), true);
                s.DefenseType = (DefenseType)Enum.Parse(typeof(DefenseType), d[6].Trim(), true);

                map[s.SkillIDHash] = currentIndex++;
                list.Add(s);
            }
        }
        db.Skills = list.ToArray();
        EditorUtility.SetDirty(db);
        return map;
    }

    private static void ImportEntities(Dictionary<int, int> skillMap)
    {
        var db = GetOrCreateSO<EntityDatabase>(ENTITY_SO);
        List<EntityEntry> list = new List<EntityEntry>();

        string[] lines = File.ReadAllLines(ENTITY_CSV);
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            int firstQuote = line.IndexOf('"');
            int lastQuote = line.LastIndexOf('"');

            string[] stats;
            string skillPart = "";

            if (firstQuote != -1 && lastQuote != -1)
            {
                skillPart = line.Substring(firstQuote + 1, lastQuote - firstQuote - 1); //fq = 5 , lq= 9일때 문자열속 데이터는 6~8, fq+1(6)부터 lq-fq-1(9-5-1=3 <- 6,7,8)=3개 추출
                string basePart = line.Remove(firstQuote, lastQuote - firstQuote + 1);
                stats = basePart.Split(',', StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                stats = line.Split(',');
            }

            List<int> sIndices = new List<int>();
            string[] skillIDs = skillPart.Split(',');
            foreach (var sID in skillIDs)
            {
                string trimmedID = sID.Trim();
                if (string.IsNullOrEmpty(trimmedID)) continue;
                int sHash = HashUtility.HashMachine(trimmedID);
                if (skillMap.TryGetValue(sHash, out int idx))
                    sIndices.Add(idx);
                else
                    Debug.LogWarning($"스킬 {trimmedID}를 찾을 수 없습니다.");
            }

            EntityEntry entry = new EntityEntry();
#if UNITY_EDITOR
            entry.Editor_ID = stats[0].Trim(); // 에디터에서만 보이는 이름
#endif
            entry.EntityIDHash = HashUtility.HashMachine(stats[0].Trim()); // 런타임용 해시
            entry.MaxHp = int.Parse(stats[1].Trim());
            entry.MinSpeed = int.Parse(stats[2].Trim());
            entry.MaxSpeed = int.Parse(stats[3].Trim());
            entry.Type = (EntityType)Enum.Parse(typeof(EntityType), stats[4].Trim(), true);

#if UNITY_EDITOR
            entry.skllID1 = skillIDs[0];
            entry.skllID2 = skillIDs[1];
            entry.skllID3 = skillIDs[2];
            entry.defenseID = skillIDs[3];
#endif
            // [방어적 코드] 스킬 인덱스를 안전하게 할당 (없으면 -1)
            entry.SkillIndex1 = sIndices.Count > 0 ? sIndices[0] : -1;
            entry.SkillIndex2 = sIndices.Count > 1 ? sIndices[1] : -1;
            entry.SkillIndex3 = sIndices.Count > 2 ? sIndices[2] : -1;
            entry.Defense = sIndices.Count > 3 ? sIndices[3] : -1; // 드디어 정상적으로 들어간다!

            list.Add(entry);
        }
        db.Entities = list.ToArray();
        EditorUtility.SetDirty(db);
    }

    private static T GetOrCreateSO<T>(string path) where T : ScriptableObject
    {
        T so = AssetDatabase.LoadAssetAtPath<T>(path);
        if (so == null)
        {
            so = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(so, path);
        }
        return so;
    }
}