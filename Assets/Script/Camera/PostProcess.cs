using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcess : MonoBehaviour 
{
    [SerializeField] private Material material;
    [SerializeField] private float hitDuration;

    public static PostProcess instance;
    private Coroutine hitCoroutine;
    private WaitForEndOfFrame wait;
    private int hitID;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        wait = new WaitForEndOfFrame();
        hitID = Shader.PropertyToID("_Hit");

        this.enabled = false;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }

    public void Hit()
    {
        this.enabled = true;

        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitCoroutine());
    }

    IEnumerator HitCoroutine()
    {
        material.SetFloat(hitID, 1.0f);

        for (float f = 1.0f; f > 0.0f; f -= Time.deltaTime / hitDuration)
        {
            yield return wait;
            material.SetFloat(hitID, f);
        }

        material.SetFloat(hitID, 0.0f);

        hitCoroutine = null;
        this.enabled = false;
    }
}
