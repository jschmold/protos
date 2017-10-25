using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using static LangRoids;
using Engine.Interfaces;

namespace Engine.Activities {
    /// <summary>
    /// A collection of functions intended to be reliant on one another to achieve an end.
    /// </summary>
    public class Activity : IThinkable {
        /// <summary>
        /// A collection of resources to be stored for processing
        /// </summary>
        public Dictionary<string, object> Resources {
            get; set;
        }

        /// <summary>
        /// Intended to be called every cycle
        /// </summary>
        public Action OnThink {
            get; set;
        }
        /// <summary>
        /// Called when the operation is considered successful
        /// </summary>
        public Action OnSuccess {
            get; set;
        }
        /// <summary>
        /// Called when the operation is considered a failure
        /// </summary>
        public Action OnFail {
            get; set;
        }
        /// <summary>
        /// Called when the operation is completed, regardless of success or failure
        /// </summary>
        public Action OnCompleted {
            get; set;
        }

        /// <summary>
        /// Has the OnFail or OnSuccess been called?
        /// </summary>
        private bool Finished {
            get; set;
        }
        /// <summary>
        /// The variable storing the failure status
        /// </summary>
        public bool Failed => !Succeeded;
        /// <summary>
        /// The variable storing the Succeeded status
        /// </summary>
        public bool Succeeded {
            get; set;
        }
        /// <summary>
        /// The variable storing whether or not 
        /// </summary>
        public bool InProgress => Started && !(Failed || Succeeded);

        /// <summary>
        /// Check if the operation has been completed;
        /// </summary>
        public bool Completed => Failed || Succeeded;
        /// <summary>
        /// Whether the activity has been started.
        /// </summary>
        public bool Started {
            get; private set;
        }

        /// <summary>
        /// Create a new activity 
        /// </summary>
        /// <param name="onThink"><see cref="OnThink"/></param>
        /// <param name="onSuccess"><see cref="OnSuccess"/></param>
        /// <param name="onFail"><see cref="OnFail"/></param>
        /// <param name="onComplete"><see cref="OnCompleted"/></param>
        /// <param name="resources"><see cref="Resources"/></param>
        public Activity(Action onThink, Action onSuccess, Action onFail, Action onComplete, Dictionary<string, object> resources = null) {
            OnThink = onThink;
            OnSuccess = onSuccess;
            OnFail = onFail;
            OnCompleted = onComplete;
            Resources = resources ?? new Dictionary<string, object>( );
            Started = false;
        }

        /// <summary>
        /// Set started to true
        /// </summary>
        public void Start() => Started = true;

        /// <summary>
        /// Perform the OnFail or OnSuccess, and set Finished to true.
        /// </summary>
        private void Finish() => Perform(!Finished, () => {
            Perform(Succeeded, OnFail, OnSuccess);
            Finished = true;
        });

        /// <summary>
        /// Appropriately process the Activity
        /// </summary>
        public void Think() => Perform(
            (Started, DoNothing),
            (Completed && !Finished, Finish),
            OnThink);
    }
}