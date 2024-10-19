using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTime : MonoBehaviour
{
    public float time;
    public string scene;

    async void Start()
    {
        await UniTask.WaitForSeconds(time);
        SceneManager.LoadScene(scene);
    }
}