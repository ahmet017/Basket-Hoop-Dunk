using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public static class MonoBehaviourExtensions
{

    public static Coroutine WaitForSeconds(this MonoBehaviour self, float t, UnityAction callback)
    {
        return self.StartCoroutine(WaitForSecondsCoroutine(t, callback));
    }


    static IEnumerator WaitForSecondsCoroutine(float t, UnityAction callback)
    {
        yield return new WaitForSeconds(t);
        callback.Invoke();
    }

    public static void RestartComponent(this MonoBehaviour self)
    {
        self.enabled = false;
        self.enabled = true;
    }

}
