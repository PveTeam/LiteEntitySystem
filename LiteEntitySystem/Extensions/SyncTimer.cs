﻿using LiteEntitySystem.Internal;

namespace LiteEntitySystem.Extensions
{
    public class SyncTimer : SyncableField
    {
        public float MaxTime => _maxTime;
        public float ElapsedTime => _time;
        public bool IsTimeElapsed => _time >= _maxTime;
        
        private SyncVar<float> _time;
        private SyncVar<float> _maxTime;

        public SyncTimer(float maxTime) 
        {
            _maxTime = maxTime;
            Finish();
        }

        public SyncTimer() 
        {
            
        }

        public float Progress
        {
            get
            {
                float p = _time/_maxTime;
                return p > 1f ? 1f : p;
            }
        }

        public void Reset()
        {
            _time = 0f;
        }

        public void Reset(float maxTime)
        {
            _maxTime = maxTime;
            _time = 0f;
        }

        public void Finish()
        {
            _time = _maxTime;
        }

        public float LerpByProgress(float a, float b)
        {
            return Utils.Lerp(a, b, Progress);
        }

        public float LerpByProgress(float a, float b, bool inverse)
        {
            return inverse
                ? Utils.Lerp(a, b, Progress)
                : Utils.Lerp(b, a, Progress);
        }

        public bool UpdateAndCheck(float delta)
        {
            if (IsTimeElapsed)
                return false;
            return Update(delta);
        }

        public bool Update(float delta)
        {
            if (_time < _maxTime)
                _time += delta;
            return IsTimeElapsed;
        }

        public bool CheckAndSubtractMaxTime()
        {
            if (_time >= _maxTime)
            {
                _time -= _maxTime;
                return true;
            }
            return false;
        }

        public bool UpdateAndReset(float delta)
        {
            if (Update(delta))
            {
                _time -= _maxTime;
                return true;
            }
            return false;
        }
    }
}