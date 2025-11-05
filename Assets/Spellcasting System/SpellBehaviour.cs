using UnityEngine;

namespace Spellcasting_System
{
    public interface SpellBehaviour
    {
        public void Init(Spell spell, Transform castPoint);
    }
}