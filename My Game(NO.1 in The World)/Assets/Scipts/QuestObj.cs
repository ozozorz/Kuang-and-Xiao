using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class QuestObject
    {
        public string questTitle;
        public string QuestObj1;
        public string QuestObj2;
    }
    public class QuestObj : MonoBehaviour
    {
        public GameObject questObj;
        public TextMeshProUGUI QuestTitle, QuestObjective1, QuestObjective2;


        public QuestObject[] questObs;
        public void StartNewQuest(QuestObject tempobj)
        {
            QuestTitle.text = tempobj.questTitle;
            QuestObjective1.text = tempobj.QuestObj1;
            QuestObjective2.text = tempobj.QuestObj2;
            questObj.SetActive(true);
            Invoke("CloseQuest", 7f);
        }

        private void CloseQuest()
        {
            questObj.SetActive(false);
        }

    }
}
