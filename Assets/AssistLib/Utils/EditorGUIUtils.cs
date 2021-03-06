﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorGUIUtils {
    public static float GetDisplayNameFieldWidth(float fieldWidth) {
        float minPropertyWidth = 250f;
        float minDisplayNameWidth = 150f;
        float displayNameScale = .42f;

        return fieldWidth < minPropertyWidth ? minDisplayNameWidth : fieldWidth * displayNameScale;
    }
}
