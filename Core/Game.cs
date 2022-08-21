using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Core
{
    public abstract class Game<TGameSetting> where TGameSetting:new()
    {

        public TGameSetting Setting { get; set; }

        public GameState CurrentState { get; protected set; }

        public event Action<object> Started;

        public event Action<object> Paused;

        public event Action<object> Overed;

        public Game(TGameSetting setting)
        {
            Setting = setting;
        }

        public Game()
        {

        }

        public virtual void Start() 
        {
            if (CurrentState == GameState.Start) return;

            OnStart();

            Started?.Invoke(this);

            CurrentState = GameState.Start;

        }

        protected virtual void OnStart() {}

        public virtual void Pause()
        {
            if (CurrentState != GameState.Start) return;

            OnPause();

            Paused?.Invoke(this);

            CurrentState = GameState.Pause;
        }
        protected virtual void OnPause() { }

        public virtual void Over() 
        {
            if (CurrentState == GameState.Over) return;

            OnOver();

            Overed?.Invoke(this);

            CurrentState = GameState.Over;
        }
        protected virtual void OnOver() { }
    }
}
