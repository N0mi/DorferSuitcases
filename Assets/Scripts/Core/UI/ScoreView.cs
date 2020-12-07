using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace UI
{
    public class ScoreView : MonoBehaviour
    {
        private Text ValueView;

        private void Start()
        {
            ValueView = GetComponent<Text>();           
            ScoreManager.instance.ChangeScore += ChangeView;
            ValueView.text = ScoreManager.instance.Score.ToString();
        }

        private void ChangeView()
        {
            ValueView.text = ScoreManager.instance.Score.ToString();
        }

        private void OnDestroy()
        {
            ScoreManager.instance.ChangeScore -= ChangeView;
        }
    }
}

