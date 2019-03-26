using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace AillieoUtils
{


    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if(m_instance == null)
                {
                    CreateInstance();
                }
                return m_instance;
            }
        }

        public static void CreateInstance()
        {
            if (m_instance == null)
            {
                var go = new GameObject(string.Format("[{0}]", typeof(T).Name));
                m_instance = go.AddComponent<T>();
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    GameObject.DontDestroyOnLoad(go);
                }
                else
                {
                    go.hideFlags = HideFlags.HideAndDontSave;
                    EditorApplication.playModeStateChanged += stateChanged =>
                    {
                        if (m_instance != null)
                        {
                            GameObject.DestroyImmediate(m_instance.gameObject);
                            m_instance = null;
                        }
                    };
                }
#else
                GameObject.DontDestroyOnLoad(go);
#endif
            }
        }

        protected void OnDestroy()
        {
            m_instance = null;
        }

        protected void OnApplicationQuit()
        {
            if (m_instance != null)
            {
#if UNITY_EDITOR
                GameObject.DestroyImmediate(m_instance.gameObject);
#else
                GameObject.Destroy(m_instance.gameObject);
#endif

            }
        }
    }
}
