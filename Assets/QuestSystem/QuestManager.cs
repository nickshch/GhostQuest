﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace QuestSystem
{
    public class QuestManager
    {
        private const string _QUEST_FOLDER = "Quests/";
        private const string _SERIALIZATION_FILE ="QuestData.bin";

        private static Dictionary<string, Quest> _quests;


        public static void StartQuest(string questTitle)
        {
            string questJson = Resources.Load<TextAsset>(_QUEST_FOLDER + questTitle).text;
            var quest = QuestParser.Parse(questJson);
            _quests.Add(quest.Title, quest);
        }

        public static void ShowQuestNote(string questTitle, uint noteId)
        {
            _quests[questTitle].SetNoteVisible(noteId, true);
        }

        public static QuestTask GetTask(string questTitle, uint taskId)
        {
            return _quests[questTitle].Tasks[taskId];
        }

        public static QuestTask[] GetTasks(string questTitle)
        {
            return _quests[questTitle].Tasks;
        }

        public static void ShowTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsVisible = true;
        }

        public static void DoTask(string questTitle, uint taskId)
        {
            _quests[questTitle].Tasks[taskId].IsDone = true;
        }

        public static IEnumerable<QuestTask> GetVisibleTasks(string questTitle)
        {
            return _quests[questTitle].Tasks.Where((task) => task.IsVisible);
        }

        public static IEnumerable<string> GetVisibleNotes(string questTitle)
        {
            var quest = _quests[questTitle];
            for (uint i = 0; i < quest.QuestNotes.Length; i++)
            {
                if (quest.isNoteVisible(i))
                {
                    yield return quest.QuestNotes[i];
                }
            }
        }

        public static IEnumerable<string> QuestTitles
        {
            get { return _quests.Values.Select(quest => quest.Title); }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void DownloadQuests()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream ( _SERIALIZATION_FILE, FileMode.Open, FileAccess.Read, FileShare.Read);
            _quests = (Dictionary<string, Quest>) formatter.Deserialize(stream);
            stream.Close();
        }

        public static void SaveQuests()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream( _SERIALIZATION_FILE, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, _quests);
            stream.Close();
        }
    }
}