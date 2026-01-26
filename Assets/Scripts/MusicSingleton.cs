using FMODUnity;
using UnityEngine;

public class MusicSingleton : MonoBehaviour
{
    public static MusicSingleton MusicInstance;
    [SerializeField] private StudioEventEmitter FMODMusic;

    void Awake()
    {
        if (MusicInstance != null && MusicInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        MusicInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FMODMusic.Play();
    }
}
