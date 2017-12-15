using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour 
{
    [SerializeField] private float delayBeforeAttract;
    [SerializeField] private float attractForce;
    [SerializeField] private float attractRange;
    [SerializeField] private float grabRange;
    [SerializeField] private ParticleSystem pSystem;
    [SerializeField] private AudioSource recupSound;
    [SerializeField] private Vector2 recupSoundRandomPitch;

    public int currentScore;
    
    private ParticleSystem.Particle[] particles;
    private Transform playerTransform;
    private Transform trsf;
    private bool recup;

    public static MoneyManager instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Instantiate<MoneyManager>(Resources.Load<MoneyManager>("MoneyManager"));
            }

            return s_instance;
        }
    }

    private static MoneyManager s_instance;

	void Awake () 
    {
        DontDestroyOnLoad(gameObject);
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        trsf = transform;
        currentScore = 0;
        particles = new ParticleSystem.Particle[pSystem.main.maxParticles];
        recup = false;
	}
	
	void Update () 
    {
        recup = false;
        int numParticles = pSystem.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            if (particles[i].startLifetime - particles[i].remainingLifetime < delayBeforeAttract)
                return;
                
            Vector3 dir = playerTransform.position - particles[i].position;
            float sqrMagnitude = dir.sqrMagnitude;

            if (sqrMagnitude < attractRange * attractRange)
            {
                particles[i].position = Vector3.Lerp(particles[i].position, playerTransform.position, Time.deltaTime * attractForce);

                if (sqrMagnitude < grabRange)
                {
                    particles[i].remainingLifetime = 0.0f;
                    currentScore += 1;
                    recup = true;
                }
            }

            pSystem.SetParticles(particles, numParticles);

            if (recup == true)
            {
                recupSound.pitch = Random.Range(recupSoundRandomPitch.x, recupSoundRandomPitch.y);
                recupSound.Play();
            }
        }
	}

    public void SpawnMoney(int moneyCount, Vector3 position)
    {
        trsf.position = position;
        pSystem.Emit(moneyCount);
    }
}
