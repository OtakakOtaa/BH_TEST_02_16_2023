

using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface ICoroutineHolder
    {
        delegate IEnumerator CoroutineMethod();  
        
        Coroutine ExecuteCoroutine(CoroutineMethod coroutineMethod);
        void ShutDownCoroutine(Coroutine coroutine);
    }
}