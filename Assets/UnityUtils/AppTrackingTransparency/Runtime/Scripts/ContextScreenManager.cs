using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

#if UNITY_IPHONE
using Unity.Advertisement.IosSupport;
#endif

namespace UnityUtils.AppTrackingTransparency
{
    /// <summary>
    /// This component will trigger the context screen to appear when the scene starts,
    /// if the user hasn't already responded to the iOS tracking dialog.
    /// </summary>
    public class ContextScreenManager : MonoBehaviour
    {
        /// <summary>
        /// The prefab that will be instantiated by this component.
        /// The prefab has to have an ContextScreenView component on its root GameObject.
        /// </summary>
        public ContextScreenView contextScreenPrefab;
        public string scenePath;

        void Start()
        {
#if UNITY_IPHONE
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                var contextScreen = Instantiate(contextScreenPrefab).GetComponent<ContextScreenView>();
                contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
            }
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
#if UNITY_IPHONE && !UNITY_EDITOR
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                yield return null;
            }
#endif
            SceneManager.LoadScene(scenePath);
            yield return null;
            
        }
    }   
}
