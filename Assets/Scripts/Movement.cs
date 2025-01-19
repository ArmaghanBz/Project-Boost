using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustspeed = 10f;
    [SerializeField] float rotationspeed = 10f;
    [Header("Audio & Particles")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThurstParticles;
    [SerializeField] ParticleSystem rightThurstParticles;
    Rigidbody rb;
    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        AddForce();
        AddRotation();
    }

    private void AddForce()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustspeed * Time.fixedDeltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void AddRotation()
    {
       float rotationThisFrame = rotation.ReadValue<float>();
       if(rotationThisFrame < 0)
       {
            Rotate(1);
            if(!rightThurstParticles.isPlaying)
            {
                rightThurstParticles.Play();
            }
            
       }
       else if(rotationThisFrame > 0)
       {
            Rotate(-1); 
            if(!leftThurstParticles.isPlaying)
            {
                leftThurstParticles.Play();
            }
       }
       else
       {
            rightThurstParticles.Stop();
            leftThurstParticles.Stop();
       }
    }

    void Rotate(float direction)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * direction * rotationspeed * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
