using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeTexture : MonoBehaviour
{
    [SerializeField]
    Sprite[] possibleTextures;

    [SerializeField]
    Vector2 randomRotation = Vector2.zero;

    [SerializeField]
    Vector2 randomSize = Vector2.one;

    float wantedSize, wantedRotation;

    [SerializeField]
    float fadeInTime;

    [SerializeField]
    AnimationCurve fadeCurve;

    [Header("Update Methods")]
    [SerializeField]
    bool changeOverTime;
    float rTime, sTime;
    [SerializeField]
    Vector2 sizeLimits;
    [SerializeField]
    Vector2 sizeChangeSpeedLimit;
    public float sizeChangeSpeed;
    [SerializeField]
    AnimationCurve sizeChange;
    [SerializeField]
    Vector2 rotationLimits;
    [SerializeField]
    Vector2 rotationChangeSpeedLimit;
    public float rotationChangeSpeed;
    [SerializeField]
    AnimationCurve rotationChange;
    bool isFading;


    void Start()
    {
        if (TryGetComponent(out SpriteRenderer sr))
        {
            sr.sprite = possibleTextures[Random.Range(0, possibleTextures.Length - 1)];

        }
        else if (TryGetComponent(out Image image))
        {
            image.sprite = possibleTextures[Random.Range(0, possibleTextures.Length - 1)];
        }
        wantedRotation = Random.Range(randomRotation.x, randomRotation.y);
        gameObject.transform.eulerAngles += new Vector3(transform.rotation.x, transform.rotation.y, wantedRotation);
        wantedSize = Random.Range(randomSize.x, randomSize.y);

        if (fadeInTime != 0)
        {
            StartCoroutine(FadeIn());
        }

        if (changeOverTime)
        {
            rotationChangeSpeed = Random.Range(rotationChangeSpeedLimit.x, rotationChangeSpeedLimit.y);
            sizeChangeSpeed = Random.Range(sizeChangeSpeedLimit.x, sizeChangeSpeedLimit.y);
        }
    }

    private void Update()
    {
        if (!changeOverTime || isFading)
            return;
        rTime += Time.deltaTime * rotationChangeSpeed;
        sTime += Time.deltaTime * sizeChangeSpeed;
        if (rTime > 1)
        {
            rTime--;
        }
        if (sTime > 1)
        {
            sTime--;
        }
        //if (cTime > 1)
        //    cTime--;
        transform.localScale = Vector3.one * Mathf.Lerp(sizeLimits.x, sizeLimits.y, sizeChange.Evaluate(sTime) * wantedSize);
        transform.eulerAngles = Vector3.forward * (Mathf.Lerp(rotationLimits.x, rotationLimits.y, rotationChange.Evaluate(rTime)) + wantedRotation);
    }

    private IEnumerator FadeIn()
    {
        Image currentImage = GetComponent<Image>();
        isFading = true;
        for (float i = 0; i < fadeInTime; i += Time.deltaTime)
        {
            transform.localScale = Vector3.one * fadeCurve.Evaluate(i / fadeInTime) * wantedSize;
            yield return new WaitForEndOfFrame();
        }
        isFading = false;
        transform.localScale = Vector3.one * fadeCurve.Evaluate(1) * wantedSize;
    }

    //private IEnumerator FadeIn()
    //{
    //    Image currentImage = GetComponent<Image>();
    //    Vector4 color = currentImage.color;
    //    for (float i = 0; i < fadeInTime; i += Time.deltaTime)
    //    {
    //        color.w = i / fadeInTime;
    //        currentImage.color = color;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    color.w = 1;
    //    currentImage.color = color;
    //}
}
