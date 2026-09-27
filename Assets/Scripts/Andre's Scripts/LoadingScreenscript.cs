using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadingScreenscript : MonoBehaviour
{
    [Header("Komponente")] 
    public VideoPlayer videoPlayer;
    public string videoLaden = "Saloon";

    private AsyncOperation _ladeVorgang;
    private bool videoGeladen = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        videoPlayer.loopPointReached += OnVideoBeendet;

        StartCoroutine(LadeHintergrundSzene());
        
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _ladeVorgang != null && _ladeVorgang.progress >= 0.9f){
            videoGeladen = true;
        }
    }

    IEnumerator LadeHintergrundSzene(){
        _ladeVorgang = SceneManager.LoadSceneAsync(videoLaden);
        _ladeVorgang.allowSceneActivation = false;
        while (_ladeVorgang.progress < 0.9f){
            yield return null; 
        }

        _ladeVorgang.allowSceneActivation = true;
    }

    void OnVideoBeendet(VideoPlayer video){
        videoPlayer.loopPointReached -= OnVideoBeendet;
        videoGeladen = true;
    }
}
