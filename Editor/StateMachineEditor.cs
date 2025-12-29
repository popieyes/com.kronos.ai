using UnityEditor;
using UnityEngine;

namespace Popieyes.AI.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : UnityEditor.Editor
    {
        private bool _showStates = true;

        public override void OnInspectorGUI()
        {
            // Draw the default stuff (like your initial state field)
            base.OnInspectorGUI();

            StateMachine sm = (target as StateMachine);
        
            EditorGUILayout.Space();
            _showStates = EditorGUILayout.BeginFoldoutHeaderGroup(_showStates, "Active States on Entity");

            if (_showStates)
            {
                // Find all components that implement IState
                MonoBehaviour[] components = sm.GetComponents<MonoBehaviour>();
            
                foreach (var comp in components)
                {
                    if (comp is IState)
                    {
                        DrawStateBox(comp);
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void DrawStateBox(MonoBehaviour state)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            Rect headerRect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(headerRect, state.GetType().Name, EditorStyles.boldLabel);

            // This draws the internal variables of that state script 
            // without showing the "Script" header or the bulky component UI
           /*  UnityEditor.Editor editor = CreateEditor(state);
            editor.OnInspectorGUI(); */

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(2);
        }
    
    }
}
