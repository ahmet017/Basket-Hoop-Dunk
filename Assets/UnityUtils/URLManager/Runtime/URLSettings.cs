using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.URLManager
{
    public class URLSettings : ScriptableObject
    {
        [HideInInspector] public string androidStoreUrl;
        [HideInInspector] public string iosStoreUrl;
        [HideInInspector] public string privacyPolicyUrl;
        [HideInInspector] public List<PrivacyPolicyUrl> privacyPolicyUrls;
    }


    [System.Serializable]
    public class PrivacyPolicyUrl
    {
        public SystemLanguage language;
        public string url;
    }

}
