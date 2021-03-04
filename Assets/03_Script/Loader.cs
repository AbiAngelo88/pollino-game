
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public static class Loader
{

    //dummy class
    private class LoadingMonoBehaviour : MonoBehaviour {}
    private static float loadingProgress;
    private static float transitionDuration = 1f;

    // Gestione evento loadScene
    public delegate void LoadScene(Loader.Scene scene);
    public static event LoadScene OnLoadScene;

    public enum Scene
    {
        LoadingScene, MenuScene, Level_01
    }

    private static Action onLoaderCallback;


    // Il modo giusto per usare loadAsync è la coroutine.
    // Per startare una coroutine però è necessario associare una istanza di uno script che estende monobehaviour
    // Per questio creiamo un oggetto dummy
    private static IEnumerator loadSceneAsync(Scene scene)
    {

        loadingProgress = 0f;

        yield return null;

        AsyncOperation loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!loadingAsyncOperation.isDone)
        {
            loadingProgress = Mathf.Clamp01(loadingAsyncOperation.progress / 0.9f);
            Debug.Log(loadingProgress);
            // Fino a che l'operazione di caricamento della scena non è completo non si mostra la nuova scena
            yield return null;
        }
    }

    public static float getLoadingProgress()
    {
        return loadingProgress;
    }

    public static void loadScene(Scene scene)
    {
        GameObject dummy = new GameObject("Loading Game object");
        dummy.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneEmitter(scene));
        
    }

    // Funzione richiamata dall'esterno per caricare in modo async una scena. Nell'attesa di caricamento mostra una LoadingScene con una progressBar.
    private static IEnumerator LoadSceneEmitter(Scene scene)
    {
        
        // Si crea una funzione di callBack che verrà chiamata al primo frame di update del GameObject LoaderCallback all'interno della loadingScene.
        onLoaderCallback = () =>
        {
            // Funzione che crea un GameObject dummy che estenda MonoBehaviour altrimenti non si può usare la coroutine
            GameObject dummy = new GameObject("Loading Game object");

            dummy.AddComponent<LoadingMonoBehaviour>().StartCoroutine(loadSceneAsync(scene));
        };

        yield return null;
        // Decommentare se si decide di reinserire il crossfade ad inizio e fine scena
        // OnLoadScene?.Invoke(scene);
        // yield return new WaitForSeconds(transitionDuration);

        // Si carica la LoadingScene
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }


    // Funzione richiamata al primo frame di update dal GameObject LoaderCallback all'interno della loadingScene.
    public static void loaderCallback()
    {
        if(onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
