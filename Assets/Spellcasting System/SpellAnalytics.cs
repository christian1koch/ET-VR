using System.Diagnostics;
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

        public void RecordSpellHit()
        {
            _spellsHitTarget++;
        }
        
        public void LogStatistics()
        {
            int spellsMissed = _spellsFired - _spellsHitTarget;
            float hitRate = _spellsFired > 0 ? (_spellsHitTarget / (float)_spellsFired * 100) : 0;
            
            UnityEngine.Debug.Log($"=== SPELL ANALYTICS ===\n" +
                $"Spells Fired: {_spellsFired}\n" +
                $"Spells Hit Target: {_spellsHitTarget}\n" +
                $"Spells Missed: {spellsMissed}\n" +
                $"Hit Rate: {hitRate:F2}%\n" +
                $"Session Duration: {_sessionTimer.Elapsed:hh\\:mm\\:ss}\n" +
                $"======================");
        }

        public void ResetStatistics()
        {
            _spellsFired = 0;
            _spellsHitTarget = 0;
            _sessionTimer.Restart();
            UnityEngine.Debug.Log("SpellAnalytics reset.");
        }
    }
}

