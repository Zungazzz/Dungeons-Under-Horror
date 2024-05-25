using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve; 
    [SerializeField] private float heightY = 5f;
    [SerializeField] private GameObject mortarShadowPrefab ;
    [SerializeField] private GameObject mortarSplatter;
    // Start is called before the first frame update
    AudioManager audioManager;
    void Awake()
    {
        audioManager = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        GameObject mortarShadow = Instantiate(mortarShadowPrefab, transform.position + new Vector3(0,-0.3f,0), Quaternion.identity);
        Vector3 targetPosition = PlayerController.Instance.transform.position;
        Vector3 mortarShadowStartPosition = targetPosition + new Vector3(0,-0.3f,0);
        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPosition));
        StartCoroutine(MoveMortarShadowRoutine(mortarShadow, mortarShadowStartPosition, targetPosition));
    }
    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        while(timePassed< duration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);
            transform.position = Vector2.Lerp(startPosition, endPosition, linearT)+ new Vector2(0f,height);
            yield return null;
            
        }
        Instantiate(mortarSplatter, transform.position, Quaternion.identity);
        audioManager.PlaySFX(audioManager.Explosion);
        Destroy(gameObject);
    }
    private IEnumerator MoveMortarShadowRoutine(GameObject mortarShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        while(timePassed< duration){
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);
            mortarShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT)+ new Vector2(0f,height);
            yield return null;
            
        }
        Destroy(mortarShadow);
    }
}
