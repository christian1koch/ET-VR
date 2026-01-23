using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Spellcasting_System
{
    public class SpellAnalytics : MonoBehaviour
    {
        private static SpellAnalytics _instance;
        public static SpellAnalytics Instance => _instance;

        private int _spellsFired;
        private int _spellsHitTarget;
        private Stopwatch _sessionTimer = new Stopwatch();
        private bool? _isUsingHandTracking;
        
        // Per-spell-type tracking
        private Dictionary<SketchType, int> _spellsFiredByType = new Dictionary<SketchType, int>();
        private Dictionary<SketchType, int> _spellsHitByType = new Dictionary<SketchType, int>();

        [SerializeField] private bool logOnDestroy = true;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            ResetStatistics();
            UnityEngine.Debug.Log("SpellAnalytics started tracking...");
        }

      
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _sessionTimer.Stop();
                LogFinalStatistics();
                _instance = null;
            }
        }

        private void OnApplicationQuit()
        {
            UnityEngine.Debug.Log("Quitting Application...");
            LogFinalStatistics();
        }

        private void LogFinalStatistics()
        {
            LogStatistics();
        }
        

        public void RecordSpellFired()
        {
            _spellsFired++;
        }
        
        public void RecordSpellFired(SketchType spellType)
        {
            _spellsFired++;
            if (!_spellsFiredByType.ContainsKey(spellType))
                _spellsFiredByType[spellType] = 0;
            _spellsFiredByType[spellType]++;
        }

        public void RecordSpellHit()
        {
            _spellsHitTarget++;
        }
        
        public void RecordSpellHit(SketchType spellType)
        {
            _spellsHitTarget++;
            if (!_spellsHitByType.ContainsKey(spellType))
                _spellsHitByType[spellType] = 0;
            _spellsHitByType[spellType]++;
        }
        
        public void SetTrackingMode(bool isUsingHandTracking)
        {
            _isUsingHandTracking = isUsingHandTracking;
        }
        
        public void LogStatistics()
        {
            int spellsMissed = _spellsFired - _spellsHitTarget;
            float hitRate = _spellsFired > 0 ? (_spellsHitTarget / (float)_spellsFired * 100) : 0;
            string trackingMode = _isUsingHandTracking.HasValue 
                ? (_isUsingHandTracking.Value ? "Hand Tracking" : "Controller Tracking")
                : "Unknown";
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== SPELL ANALYTICS ===");
            sb.AppendLine($"Input Mode: {trackingMode}");
            sb.AppendLine($"Session Duration: {_sessionTimer.Elapsed:hh\\:mm\\:ss}");
            sb.AppendLine();
            sb.AppendLine("--- Overall Stats ---");
            sb.AppendLine($"Spells Fired: {_spellsFired}");
            sb.AppendLine($"Spells Hit Target: {_spellsHitTarget}");
            sb.AppendLine($"Spells Missed: {spellsMissed}");
            sb.AppendLine($"Hit Rate: {hitRate:F2}%");
            sb.AppendLine();
            sb.AppendLine("--- Per Spell Type ---");
            
            // Log stats for each spell type
            foreach (SketchType spellType in System.Enum.GetValues(typeof(SketchType)))
            {
                int fired = _spellsFiredByType.ContainsKey(spellType) ? _spellsFiredByType[spellType] : 0;
                int hit = _spellsHitByType.ContainsKey(spellType) ? _spellsHitByType[spellType] : 0;
                int missed = fired - hit;
                
                sb.AppendLine($"{spellType}:");
                sb.AppendLine($"  Casted: {fired}");
                sb.AppendLine($"  Hit: {hit}");
                sb.AppendLine($"  Missed: {missed}");
            }
            
            sb.AppendLine("======================");
            
            UnityEngine.Debug.Log(sb.ToString());
        }

        public void ResetStatistics()
        {
            _spellsFired = 0;
            _spellsHitTarget = 0;
            _spellsFiredByType.Clear();
            _spellsHitByType.Clear();
            _sessionTimer.Restart();
            UnityEngine.Debug.Log("SpellAnalytics reset.");
        }
    }
}

