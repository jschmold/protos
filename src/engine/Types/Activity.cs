using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static LangRoids;


namespace Engine {
    /// <summary>
    /// An action performed on every think, with an optional "onfinish" to be performed.
    /// </summary>
    public class Activity : IThinkable {
        /// <summary>
        /// Whether or not the operation has started
        /// </summary>
        public bool Started {
            get; private set;
        }
        /// <summary>
        /// Whether or not the operation has been finished
        /// </summary>
        public bool Finished {
            get; private set;
        }
        /// <summary>
        /// Whether or not the operation has been started, but has not finished yet.
        /// </summary>
        public bool InProgress => Started && !Finished;

        private Action OnThink {
            get; set;
        }

        private Action OnFinish {
            get; set;
        }

        /// <summary>
        /// <see cref="IThinkable.Think"/>
        /// </summary>
        public void Think() => Perform(InProgress, OnThink);

        /// <summary>
        /// Set this activity to Finished, then call OnFinish if OnFinish has been set.
        /// </summary>
        public void Complete() => Perform(!Finished, () => {
            Finished = true;
            OnFinish?.Invoke( );
        });

        /// <summary>
        /// Create a new activity with an OnThink.
        /// </summary>
        /// <param name="onThink"></param>
        /// <param name="started"></param>
        public Activity(Action onThink, bool started = false) {
            OnThink = onThink;
            Started = started;
            Finished = false;
        }

        /// <summary>
        /// Create a new activity with an OnThink and an OnFinished.
        /// </summary>
        /// <param name="onThink"></param>
        /// <param name="onFinished"></param>
        /// <param name="started"></param>
        public Activity(Action onThink, Action onFinished, bool started = false) 
            : this(onThink, started) 
            => OnFinish = onFinished; 
    }
}