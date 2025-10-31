using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

namespace Potency.Services.Runtime.Utils.Logging.Editor
{
    public enum MLogLevel
    {
        Verbose,
        Info, 
        Warn,
        Error,
    }
    
    /// <summary>
    /// Adds menu items to switch between different logging levels
    /// </summary>
    [InitializeOnLoad]
    public class PLogLevelEditor
    {
        private const string SymbolError = "LOG_LEVEL_ERROR";
        private const string SymbolWarn = "LOG_LEVEL_WARN";
        private const string SymbolInfo = "LOG_LEVEL_INFO";
        private const string SymbolVerbose = "LOG_LEVEL_VERBOSE";

        private const string MenuNameNone = "Potent/Logging/None";
        private const string MenuNameError = "Potent/Logging/Error";
        private const string MenuNameWarn = "Potent/Logging/Warning";
        private const string MenuNameInfo = "Potent/Logging/Info";
        private const string MenuNameVerbose = "Potent/Logging/Verbose";

        private static readonly List<string> _currentDefineSymbols;

        static PLogLevelEditor()
        {
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            var symbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

            _currentDefineSymbols = symbols.Split(';').ToList();
        }

        [MenuItem(MenuNameNone)]
        private static void SetLevelNone()
        {
            SetSelectedLevel(null);
        }

        [MenuItem(MenuNameError)]
        private static void SetLevelError()
        {
            SetSelectedLevel(MLogLevel.Error);
        }

        [MenuItem(MenuNameWarn)]
        private static void SetLevelWarn()
        {
            SetSelectedLevel(MLogLevel.Warn);
        }

        [MenuItem(MenuNameInfo)]
        private static void SetLevelInfo()
        {
            SetSelectedLevel(MLogLevel.Info);
        }

        [MenuItem(MenuNameVerbose)]
        private static void SetLevelVerbose()
        {
            SetSelectedLevel(MLogLevel.Verbose);
        }

        [MenuItem(MenuNameNone, true)]
        private static bool SetLevelNoneValidate()
        {
            return ValidateSetLevel(MenuNameNone, null);
        }

        [MenuItem(MenuNameError, true)]
        private static bool SetLevelErrorValidate()
        {
            return ValidateSetLevel(MenuNameError, MLogLevel.Error);
        }

        [MenuItem(MenuNameWarn, true)]
        private static bool SetLevelWarnValidate()
        {
            return ValidateSetLevel(MenuNameWarn, MLogLevel.Warn);
        }

        [MenuItem(MenuNameInfo, true)]
        private static bool SetLevelInfoValidate()
        {
            return ValidateSetLevel(MenuNameInfo, MLogLevel.Info);
        }

        [MenuItem(MenuNameVerbose, true)]
        private static bool SetLevelVerboseValidate()
        {
            return ValidateSetLevel(MenuNameVerbose, MLogLevel.Verbose);
        }

        private static MLogLevel? GetSelectedLevel()
        {
            if(_currentDefineSymbols.Contains(SymbolError))
                return MLogLevel.Error;

            if(_currentDefineSymbols.Contains(SymbolWarn))
                return MLogLevel.Warn;

            if(_currentDefineSymbols.Contains(SymbolInfo))
                return MLogLevel.Info;

            if(_currentDefineSymbols.Contains(SymbolVerbose))
                return MLogLevel.Verbose;

            return null;
        }

        private static void SetSelectedLevel(MLogLevel? level)
        {
            _currentDefineSymbols.Remove(SymbolError);
            _currentDefineSymbols.Remove(SymbolWarn);
            _currentDefineSymbols.Remove(SymbolInfo);
            _currentDefineSymbols.Remove(SymbolVerbose);

            switch(level)
            {
                case MLogLevel.Error:
                    _currentDefineSymbols.Add(SymbolError);
                    break;
                case MLogLevel.Warn:
                    _currentDefineSymbols.Add(SymbolWarn);
                    break;
                case MLogLevel.Info:
                    _currentDefineSymbols.Add(SymbolInfo);
                    break;
                case MLogLevel.Verbose:
                    _currentDefineSymbols.Add(SymbolVerbose);
                    break;
                case null:
                    // Do nothing - null means no logging, no symbols needed
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            var namedBuildTarget =
                NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);

            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, _currentDefineSymbols.ToArray());
        }

        private static bool ValidateSetLevel(string menuName, MLogLevel? level)
        {
            var selectedLevel = GetSelectedLevel();
            Menu.SetChecked(menuName, selectedLevel == level);
            return selectedLevel != level;
        }
    }
}