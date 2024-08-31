using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Ump;
using UnityEngine.Events;
using GoogleMobileAds.Ump.Api;
using UnityEngine.SceneManagement;

namespace UnityUtils.AdmobAdManager
{
    public class AdmobUMPManager : MonoBehaviour
    {
        public bool loadSceneAfterComplete;
        public UnityEvent onComplete = new UnityEvent();
        public string scenePath;
        public bool underAge;

        void Start()
        {

#if !UNITY_ANDROID && !UNITY_IPHONE
        StartCoroutine(Complete());
        return;
#endif
            ConsentDebugSettings debugSettings = new ConsentDebugSettings();
            debugSettings.DebugGeography = DebugGeography.Disabled;

            ConsentRequestParameters request = new ConsentRequestParameters();
            request.TagForUnderAgeOfConsent = underAge;
            request.ConsentDebugSettings = debugSettings;

            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        void OnConsentInfoUpdated(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                StartCoroutine(Complete());
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                StartCoroutine(Complete());
            });
        }

        private IEnumerator Complete()
        {
            if (loadSceneAfterComplete && !string.IsNullOrEmpty(scenePath))
            {
                yield return new WaitForSeconds(1f);
                SceneManager.LoadScene(scenePath);
            }
            else
            {
                onComplete.Invoke();
            }
        }
    }
}

