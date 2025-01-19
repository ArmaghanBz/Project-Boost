using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [Header("Audio & Particles")]
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isControllable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            loadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            reloadLevel();
        }
    }

    

    private void OnCollisionEnter(Collision other)
    {
        if(!isControllable){    return; }

       switch (other.gameObject.tag)
       {
           case "Friendly":
                Debug.Log("This thing is friendly");
                break;
           case "finish":
                Debug.Log("You've finished the level!");
                SuccessSequence();
                break;
           default:
                CrashSequence();
                 Debug.Log("You've crashed!");
                break;
       }
    }

    void SuccessSequence()
    {
        successParticles.Play();
        isControllable = false;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(success);
        
        Invoke("loadNextLevel",levelLoadDelay);
    }
    void CrashSequence()
    {
        crashParticles.Play();
        isControllable = false;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crash);
        
        Invoke("reloadLevel",levelLoadDelay);
    }

    void loadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

    }

    void reloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
