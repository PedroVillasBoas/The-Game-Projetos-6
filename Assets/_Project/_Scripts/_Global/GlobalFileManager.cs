using System;
using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using GoodVillageGames.Game.DataCollection;

namespace GoodVillageGames.Game.Core.Global
{
    public class GlobalFileManager : MonoBehaviour
    {
        public static GlobalFileManager Instance { get; private set; }

        private static readonly string FILENAME = "VoidProtocolLeaderboard.json";
        private string folderPath;
        private string filePath;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            folderPath = Path.Combine(Application.persistentDataPath, "Leaderboard");
            filePath = Path.Combine(folderPath, FILENAME);

            InitializeLeaderboardFile();
        }

        void InitializeLeaderboardFile()
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, "{}"); 
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Falha ao inicializar o diretório/arquivo de save: {e.Message}");
            }
        }

        public void SaveScoreOnFile(GameRunData gameRunData)
        {
            try
            {
                string currentJson = File.ReadAllText(filePath);

                var allScores = JsonConvert.DeserializeObject<Dictionary<string, LeaderboardData>>(currentJson) ?? new Dictionary<string, LeaderboardData>();

                var newEntry = new LeaderboardData
                {
                    SessionID = gameRunData.RunStartTime.ToString("o"),
                    TotalRunScore = gameRunData.TotalRunScore,
                    TotalRunTime = gameRunData.TotalRunTimeMinutes,
                    TotalEnemiesDefeated = gameRunData.EnemiesDefeated.Values.Sum(),
                    UpgradesCollected = gameRunData.UpgradesCollected.Values.Sum(),
                    NormalShotAccuracy = gameRunData.NormalShotAccuracy,
                    MissileShotAccuracy = gameRunData.MissileShotAccuracy
                };

                string newId = (allScores.Count + 1).ToString();
                allScores.Add(newId, newEntry);

                string updatedJson = JsonConvert.SerializeObject(allScores, Formatting.Indented);
                File.WriteAllText(filePath, updatedJson);
                
                Debug.Log($"Score succefully saved with ID: {newId}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save score on file: {e.Message}");
            }
        }

        public List<LeaderboardData> GetSavedScores()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning("Leaderboardfile not found. Returning an empty list.");
                    return new List<LeaderboardData>();
                }
                
                string json = File.ReadAllText(filePath);
                
                if (string.IsNullOrWhiteSpace(json))
                {
                     return new List<LeaderboardData>();
                }

                var allScores = JsonConvert.DeserializeObject<Dictionary<string, LeaderboardData>>(json);

                if (allScores == null || allScores.Count == 0)
                {
                    return new List<LeaderboardData>();
                }
                
                return allScores.Values.OrderByDescending(score => score.TotalRunScore).ToList();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to read file scores: {e.Message}");
                return new List<LeaderboardData>();
            }
        }
    }
}