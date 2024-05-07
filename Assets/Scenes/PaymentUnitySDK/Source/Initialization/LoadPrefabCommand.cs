using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Scenes.PaymentUnitySDK.Initialization
{
    public class LoadPrefabCommand : IPaymentCommand
    {
        private readonly string m_PrefabName;
        private GameObject m_LoadedPrefab;

        public LoadPrefabCommand(string name)
        {
            m_PrefabName = name;
        }

        public async Task ExecuteAsync()
        {
            await RunOnMainThread(() =>
                Object.FindFirstObjectByType<MonoBehaviour>().StartCoroutine(LoadResourceAsync()));
        }

        private IEnumerator LoadResourceAsync()
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(m_PrefabName);

            yield return resourceRequest;

            if (resourceRequest.asset != null)
            {
                m_LoadedPrefab = resourceRequest.asset as GameObject;
                Object.Instantiate(m_LoadedPrefab).name = m_PrefabName;
                Debug.Log($"Prefab '{m_PrefabName}' successfully loaded and instantiated.");
            }
            else
            {
                Debug.LogWarning($"Prefab '{m_PrefabName}' could not be found in Resources.");
            }
        }

        private Task RunOnMainThread(System.Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            Object.FindFirstObjectByType<MonoBehaviour>().StartCoroutine(RunCoroutine());

            IEnumerator RunCoroutine()
            {
                action();
                tcs.SetResult(true);
                yield break;
            }

            return tcs.Task;
        }
    }
}