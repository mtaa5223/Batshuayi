using System;
using UnityEngine;

namespace MalbersAnimations.IK
{
    [Serializable]
    public abstract class WeightProcessor
    {
        [HideInInspector] public bool Active = true;

        /// <summary>  Process the weight given some extra parameters to check and it multiplies to the entry weight</summary>
        /// <param name="weight">Entry weight to modify</param>
        /// <param name="set"> IK Set sending the weight</param>
        /// <returns>returns the processed weight</returns>
        public abstract float Process(IKSet set, float weight);
        public virtual void Initialize(IKSet set, Animator anim) { }

        public virtual void OnDrawGizmos(IKSet set, Animator anim) { }
        public virtual void OnHandlesGizmos(IKSet set, Animator anim) { }
    }
}
