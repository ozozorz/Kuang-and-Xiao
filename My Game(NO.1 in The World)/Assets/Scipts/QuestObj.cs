using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class QuestObject
    {
        public string questTitle;
        public string QuestObj1;
        public string QuestObj2;
        public Transform target;
    }
    public class QuestObj : MonoBehaviour
    {
        public GameObject questObj;
        public TextMeshProUGUI QuestTitle, QuestObjective1, QuestObjective2;

        public Image questMarker;
        public Transform target;

        public QuestObject currentQuest;
        
        public QuestObject[] questObjs;

        public TextMeshProUGUI distancetext;
        public void StartNewQuest(QuestObject tempobj)
        {
            QuestTitle.text = tempobj.questTitle;
            QuestObjective1.text = tempobj.QuestObj1;
            QuestObjective2.text = tempobj.QuestObj2;
            questObj.SetActive(true);
            Invoke("CloseQuest", 6.5f);

            currentQuest = tempobj;
            target = currentQuest.target;
        }

        private void Update()
        {

            if (target == null)
            {
                questMarker.gameObject.SetActive(false);
            }
            else
            {
                var distance = Vector3.Distance(transform.position, target.position);
                distancetext.text = distance.ToString("F2") + "m";
                questMarker.gameObject.SetActive(true);
                float minX = questMarker.GetPixelAdjustedRect().width / 2;
                float maxX = Screen.width - minX;

                float minY = questMarker.GetPixelAdjustedRect().height / 2;
                float maxY = Screen.height - minX;

                Vector2 pos = Camera.main.WorldToScreenPoint(target.position);

                if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
                {
                    if (pos.x < Screen.width / 2)
                    {
                        pos.x = maxX;
                    }
                    else
                    {
                        pos.x = minX;
                    }
                }

                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.y = Mathf.Clamp(pos.y, minY, maxY);

                questMarker.transform.position = pos;
            }
        }

        private void CloseQuest()
        {
            questObj.SetActive(false);
        }

    }
}
