using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using GameData.Types;

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

    private static Dictionary<string, int> ImportSkills()
    {
        var db = GetOrCreateSO<SkillDatabase>(SKILL_SO);
        List<SkillEntry> list = new List<SkillEntry>();
        Dictionary<string, int> map = new Dictionary<string, int>();

        using (var reader = new StreamReader(SKILL_CSV))
        {
            reader.ReadLine(); // 헤더 스킵
            int currentIndex = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                // 파싱 로직...
                map[s.ID] = currentIndex++;
                list.Add(s);
            }
        }
        db.Skills = list.ToArray();
        EditorUtility.SetDirty(db);
        return map;
    }

    private static void ImportEntities(Dictionary<string, int> skillMap)
    {
        var db = GetOrCreateSO<EntityDatabase>(ENTITY_SO);
        List<EntityEntry> list = new List<EntityEntry>();

        string[] lines = File.ReadAllLines(ENTITY_CSV);
        for (int i = 1; i < lines.Length; i++)
        {
            // CSV의 따옴표 처리 (스킬 리스트가 "SKILL_01,SKILL_02" 형태일 때)
            string line = lines[i].Replace("\"", "");
            string[] d = line.Split(',');

            List<int> sIndices = new List<int>();
            // 5번 열부터 끝까지가 스킬 ID들
            for (int j = 5; j < d.Length; j++)
            {
                string sID = d[j].Trim();
                if (skillMap.TryGetValue(sID, out int idx)) sIndices.Add(idx);
            }

            list.Add(new EntityEntry
            {
                EntityID = d[0].Trim(),
                MaxHp = int.Parse(d[1]),
                MinSpeed = int.Parse(d[2]),
                MaxSpeed = int.Parse(d[3]),
                Type = d[4].Trim(),
                SkillIndices = sIndices.ToArray()
            });
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