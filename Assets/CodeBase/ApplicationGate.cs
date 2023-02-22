using System;
using UnityEditor;
using UnityEngine;

namespace CodeBase
{
    public class ApplicationGate
    {
        [RuntimeInitializeOnLoadMethod]
        public static void Bootstrap()
        {
            try
            {
#if UNITY_EDITOR
                EditorWindow.focusedWindow.maximized = true;
#endif
                Screen.SetResolution(1920, 1080, true);
                Cursor.lockState = CursorLockMode.Confined;
            }
            catch (Exception ignored) 
            {
                // ignored
            }
        }
    }
}