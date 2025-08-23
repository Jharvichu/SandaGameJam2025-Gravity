using System;
using System.Collections;
using UnityEngine;

namespace Resonant.Runtime
{
    /// <summary>
    /// Prints a Debug.Log to the console
    /// </summary>
    [Serializable, Sourceless]
    public class LogReaction : ResonantReaction
    {
        /// <summary>
        /// The message to print
        /// </summary>
        public string Message;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            Debug.Log(Message);
            yield return null;
        }
    }
}