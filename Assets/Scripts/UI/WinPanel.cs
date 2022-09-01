using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinPanel : MonoBehaviour
    {
        public List<GameObject> crowns = new List<GameObject>();
        public Button restartButton;
        public List<AudioSource> crownAudio = new List<AudioSource>();
        private bool _bFinish;

        public void OnOpenWinPanel()
        {
            restartButton.enabled = false;
            int count = 0;
            foreach (CrownBehavior crown in DancingLineGameManager.Instance._listCrowns)
            {
                if (crown.bGetCrown)
                {
                    count += 1;
                }
            }
            StartCoroutine(DisplayCrown(count));
        }

        private void OnDisable()
        {
            _bFinish = false;
        }


        IEnumerator DisplayCrown(int count)
        {
            if (!_bFinish)
            {
                for (int i = 0; i < 3; i++)
                {
                    crowns[i].transform.localScale = new Vector3(0, 0, 0);
                }
                
                for (int i = 0; i < count; i++)
                {
                    iTween.ScaleTo(crowns[i],
                        iTween.Hash("time", 1.0, "easetype", iTween.EaseType.easeOutElastic, "scale",
                            new Vector3(1, 1, 1)));
                    crownAudio[i].Play();
                    yield return new WaitForSeconds(1f);
                }
                restartButton.enabled = true;
            }
            _bFinish = true;
            DancingLineGameManager.Instance.RefreshCrown();
        }
    }
}