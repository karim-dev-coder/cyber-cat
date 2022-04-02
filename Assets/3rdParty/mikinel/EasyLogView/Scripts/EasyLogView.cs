﻿using UnityEngine;
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

        protected void OnLogMessage(string logText, ConsoleMessageType type)
        {
            if (text == null)
                return;

            var tmp = text.text;
            TrimLine(ref tmp, maxLines);
            text.text = tmp;

            var hashcode = GetColorHashCode(type);
            PrintMessage(logText, hashcode);

            if (isAutoScroll)
            {
                scrollRect.verticalNormalizedPosition = 0;
                scrollRect.horizontalNormalizedPosition = 0;
            }
        }

        private void PrintMessage(string logText, string hashcode)
        {
            if (string.IsNullOrEmpty(hashcode))
            {
                text.text += $"{logText} \n";
                return;
            }

            text.text += $"<color=#{hashcode}>{logText}</color> \n";
        }

        private string GetColorHashCode(ConsoleMessageType type)
        {
            switch (type)
            {
                case ConsoleMessageType.Warning:
                    return "ffff00";
                case ConsoleMessageType.Error:
                    return "ff0000";
                case ConsoleMessageType.Success:
                    return "10FD1F";
                default:
                    return string.Empty;
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