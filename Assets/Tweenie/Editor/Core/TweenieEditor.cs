using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace YJL.Tween.Editor
{
    [CustomEditor(typeof(Tweenie))]
    public class TweenieEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
            string helpMessages = $"Active Tweeners: {Tweenie.GetNumberOfRunningTweener()}";
            EditorGUILayout.HelpBox(helpMessages, MessageType.None);
        }
    }

}