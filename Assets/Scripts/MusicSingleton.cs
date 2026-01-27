using FMODUnity;
using UnityEngine;

public class MusicSingleton : MonoBehaviour
{
    public static MusicSingleton MusicInstance;
    [SerializeField] private StudioEventEmitter FMODMusic;

    private void Start()
    {
        if (MusicInstance != null && MusicInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        MusicInstance = this;
        DontDestroyOnLoad(gameObject);

        FMODMusic.Play();
    }
}
