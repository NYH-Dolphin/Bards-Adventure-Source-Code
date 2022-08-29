﻿using System;
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


        IEnumerator DisplayCrown(int count)
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
                if (i == count - 1)
                {
                    restartButton.enabled = true;
                    yield break;
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}