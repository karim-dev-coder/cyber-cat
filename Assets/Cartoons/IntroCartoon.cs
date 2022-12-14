using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class IntroCartoon : MonoBehaviour
{
    [SerializeField] private UnityEvent onComplete;
    
    public static async UniTask Play()
    {
        // Грязный хак из-за не желания и времени разбираться почему игра паузится сама
        Time.timeScale = 1f;
        
        var introCartoonScene = UIDialogs.Instance.IntroCartoon;
        await SceneManager.LoadSceneAsync(introCartoonScene)
            .ViaLoadingScreenObservable()
            .ToUniTask();
        
        var inputInAnimatorState = FindObjectOfType<InputInAnimatorState>();
        await inputInAnimatorState.ToUniTask();
        
        PlayerPrefs.SetInt("isCartoonWatched", 1);
        var introCartoon = FindObjectOfType<IntroCartoon>();
        introCartoon.onComplete.Invoke();
    }
}
