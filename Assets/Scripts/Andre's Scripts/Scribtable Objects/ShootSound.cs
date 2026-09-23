using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "ShootSound", menuName = "Scriptable Objects/ShootSound")]
public class ShootSound : ScriptableObject
{
    [Range(0, 1f)] 
    public float volume = 1f;

    public AudioClip[] fireClips;
    public AudioClip lastShot;

    public void PlayShootingClip(AudioSource audioSource, bool isLastShot = false){
        if (isLastShot && lastShot != null){
            audioSource.PlayOneShot(lastShot, volume);
        }
        else{
            audioSource.PlayOneShot(fireClips[Random.Range(0, fireClips.Length)], volume);
        }
    }
}
