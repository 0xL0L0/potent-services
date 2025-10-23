using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Potency.Services.Runtime.Utils.Environment.Editor
{
    public enum EnvironmentTarget { Staging, Production }
    
    /// <summary>
    /// Adds menu items to switch between different logging levels
    /// </summary>
    [InitializeOnLoad]
    public class EnvironmentTargetEditor
    {
        private const string SYMBOL_STAGING = "TARGET_STAGING";
        private const string SYMBOL_PRODUCTION = "TARGET_PRODUCTION";

        private const string MENU_NAME_STAGING = "Matchday/Environment/Staging";
        private const string MENU_NAME_PRODUCTION = "Matchday/Environment/Production";
        
        private static readonly List<string> _currentDefineSymbols;
        
        static EnvironmentTargetEditor()
        {
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var symbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
            _currentDefineSymbols = symbols.Split(';').ToList();
        }
        
        [MenuItem(MENU_NAME_STAGING)] 
        private static void SetEnvStaging() => SetSelectedEnv(EnvironmentTarget.Staging);
        
        [MenuItem(MENU_NAME_PRODUCTION)] 
        private static void SetEnvProduction() => SetSelectedEnv(EnvironmentTarget.Production);
        
        [MenuItem(MENU_NAME_STAGING, true)] 
        private static bool SetEnvStagingValidate() => ValidateSetLevel(MENU_NAME_STAGING, EnvironmentTarget.Staging);
        
        [MenuItem(MENU_NAME_PRODUCTION, true)] 
        private static bool SetEnvProductionValidate() => ValidateSetLevel(MENU_NAME_PRODUCTION, EnvironmentTarget.Production);
        
         private static EnvironmentTarget? GetSelectedLevel()
        {
            if (_currentDefineSymbols.Contains(SYMBOL_STAGING)) { return EnvironmentTarget.Staging; }
            if (_currentDefineSymbols.Contains(SYMBOL_PRODUCTION)) { return EnvironmentTarget.Production; }
            return null;
        }

        private static void SetSelectedEnv(EnvironmentTarget? level)
        {
            PlayerPrefs.DeleteAll();
            
            _currentDefineSymbols.Remove(SYMBOL_STAGING);
            _currentDefineSymbols.Remove(SYMBOL_PRODUCTION);

            switch(level)
            {
                case null:
                case EnvironmentTarget.Staging:
                    _currentDefineSymbols.Add(SYMBOL_STAGING);
                    break;
                
                case EnvironmentTarget.Production:
                    _currentDefineSymbols.Add(SYMBOL_PRODUCTION);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, _currentDefineSymbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.Android, _currentDefineSymbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.iOS, _currentDefineSymbols.ToArray());
        }

        private static bool ValidateSetLevel(string menuName, EnvironmentTarget? level)
        {
            var selectedLevel = GetSelectedLevel();
            Menu.SetChecked(menuName, selectedLevel == level);
            return selectedLevel != level;
        }
    }
}