﻿using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace mikinel.easylogview
{
    public class EasyLogView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private bool isAutoScroll = true;
        [SerializeField] private int maxLines = 30;

        private void Awake()
        {
            if (text == null)
            {
                Debug.LogError($"Please attach TextMeshProUGUI");
                this.enabled = false;
                return;
            }

            if (scrollRect == null)
            {
                Debug.LogError($"Please attach ScrollView");
                this.enabled = false;
                return;
            }
        }

        private void Start()
        {
            text.text = string.Empty;
        }

        protected void OnLogMessage(string logText, string stackTrace, ConsoleMessageType type)
        {
            if (text == null)
                return;

            var tmp = text.text;
            TrimLine(ref tmp, maxLines);
            text.text = tmp;

            switch (type)
            {
                case ConsoleMessageType.Warning:
                    text.text += $"<color=#ffff00>{logText}</color> \n";
                    break;
                case ConsoleMessageType.Error:
                    text.text += $"<color=#ff0000>{logText}</color> \n";
                    break;
                case ConsoleMessageType.Success:
                    text.text += $"<color=#10FD1F>{logText}</color> \n";
                    break;
                default:
                    text.text += $"{logText} \n";
                    break;
            }

            if (isAutoScroll)
            {
                scrollRect.verticalNormalizedPosition = 0;
                scrollRect.horizontalNormalizedPosition = 0;
            }
        }

        public void ClearLog()
        {
            text.text = "Clear Log \n";
        }

        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }

        private void TrimLine(ref string s, int maxLine)
        {
            if (CountChar(s, '\n') >= maxLine)
                s = RemoveFirstLine(s);

            if (CountChar(s, '\n') >= maxLine)
                TrimLine(ref s, maxLine);
        }

        private string RemoveFirstLine(string s)
        {
            var pos = s.IndexOf('\n');
            return s.Substring(pos + 1, s.Length - pos - 1);
        }
    }
}