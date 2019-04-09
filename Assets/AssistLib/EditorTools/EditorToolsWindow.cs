﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.EditorTools {
    public class EditorToolsWindow : EditorWindow {

        private EditorToolsController _controller;

        [MenuItem("Assist/Tools")]
        public static void ShowSettingsWindow() {
            var toolWindow = (EditorToolsWindow)EditorWindow.GetWindow(typeof(EditorToolsWindow), true);
            toolWindow.Load();
        }

        private void Load() {
            _controller = new EditorToolsController();
        }

        void OnGUI() {
            GUILayout.Label("Editor tools", EditorStyles.boldLabel);

            _controller.DrawTools();

            if (GUI.changed) {
                _controller.SaveTools();
            }
        }
    }
}
