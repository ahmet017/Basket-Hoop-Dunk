using UnityEngine;


namespace UnityUtils.URLManager
{

    public static class URLManager
    {
        private static URLSettings settings;
        private static string privacyPolicyUrl;
        public static URLSettings Settings
        {
            get
            {
                if (settings != null)
                    return settings;
                settings = Resources.Load("URLSettings") as URLSettings;
                return settings;
            }
        }

        public static string StoreURL
        {
            get
            {
#if UNITY_ANDROID
            return Settings.androidStoreUrl;
#elif UNITY_IPHONE
            return Settings.iosStoreUrl;
#else
                return Settings.androidStoreUrl;
#endif
            }
        }

        public static string PrivacyPolicyURL
        {
            get
            {
                if (privacyPolicyUrl != null)
                    return privacyPolicyUrl;

                if (Settings.privacyPolicyUrls != null && Settings.privacyPolicyUrls.Count > 0)
                {
                    foreach (PrivacyPolicyUrl url in Settings.privacyPolicyUrls)
                    {
                        if (url.language == Application.systemLanguage)
                        {
                            privacyPolicyUrl = url.url;
                            break;
                        }
                    }
                }
                else
                {
                    privacyPolicyUrl = Settings.privacyPolicyUrl;
                }
                if (string.IsNullOrEmpty(privacyPolicyUrl))
                    privacyPolicyUrl = Settings.privacyPolicyUrl;

                return privacyPolicyUrl;
            }
        }
    }
}
