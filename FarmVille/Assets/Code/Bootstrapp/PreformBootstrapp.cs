using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Code.Bootstrapp
{
    public static class PreformBootstrapp
    {
        const string c_bootstrappSceneName = "Bootstrapp";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            for(int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var loadCandidate = SceneManager.GetSceneAt(sceneIndex);

                if (loadCandidate.name == c_bootstrappSceneName)
                    return;
            }
            Debug.Log("Looading bootstrapp!");

            SceneManager.LoadScene(c_bootstrappSceneName, LoadSceneMode.Additive);
        }
    }
}
