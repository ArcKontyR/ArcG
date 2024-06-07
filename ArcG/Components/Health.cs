using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public class Health
    {
        private int _current = 100;
        private int _max = 100;
        private int _min = 0;

        public bool IsAlive { get; private set;}
        public int Current { get => _current; }
        public Health(int min=0, int max=100)
        {
            _min = min;
            _max = max;
            IsAlive = true;
        }
        public void Damage(int value)
        {   
            if (value < _min) return;
            if (value > _max)
            {
                _current = 0;
                IsAlive = false;
                return;
            }
            _current -= value;
            if (_current <= _min)
            {
                _current = _min;
                IsAlive = false;
            }
        }

        public void Heal(int value)
        {
            if (!IsAlive) return;
            if (value < _min) return;
            if (value > _max)
            {
                _current = _max;
                return;
            }
            _current += value;
            if (_current > _max) _current = _max;
        }
    }
}
