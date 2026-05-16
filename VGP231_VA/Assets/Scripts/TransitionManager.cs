using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class TransitionManager : MonoBehaviour
{
    private enum TransitionTypes
    {
        Fade,
        None
    }

    [System.Serializable]
    private struct Transition
    {
        public Image transitionObject;
        public TransitionTypes transitionType;

        public float inDuration;
        public float outDuration;
        public float transitionTriggerDelay;
        public float sceneTriggerDelay;
    }

    [SerializeField] private Transition[] transitions;
    private Queue<Transition> transitionQueue;

    [SerializeField] private bool startSceneWithTransition = false;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        transitionQueue = new Queue<Transition>(transitions);

        if (startSceneWithTransition)
        {
            StartSceneTransition();
        }
    }

    public void StartSceneTransition()
    {
        if (transitions.Length > 0)
        {
            Transition transition = transitionQueue.Peek();

            if (transition.transitionType == TransitionTypes.Fade)
            {
                Image image = transition.transitionObject;

                Color color = image.color;

                // Start fully black immediately
                color.a = 1f;

                image.color = color;
            }

            StartCoroutine(TransitionToScene(""));
        }
    }

    public void TransitionToSceneEvent(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    public IEnumerator TransitionToScene(string sceneToTransitionTo)
    {
        if (transitionQueue.Count <= 0)
        {
            yield break;
        }

        Transition transition = transitionQueue.Dequeue();

        yield return new WaitForSeconds(transition.transitionTriggerDelay);

        switch (transition.transitionType)
        {
            case TransitionTypes.None:
                break;

            case TransitionTypes.Fade:
                {
                    Image image = transition.transitionObject;

                    Color color = image.color;

                    bool isFadeIn = transition.inDuration > 0f;
                    float duration = isFadeIn ? transition.inDuration : transition.outDuration;

                    float startAlpha = isFadeIn ? 1f : 0f;
                    float endAlpha = isFadeIn ? 0f : 1f;

                    color.a = startAlpha;
                    image.color = color;

                    float timer = 0f;

                    while (timer < duration)
                    {
                        timer += Time.deltaTime;

                        float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);

                        color.a = alpha;
                        image.color = color;

                        yield return null;
                    }

                    color.a = endAlpha;
                    image.color = color;
                }
                break;
        }

        yield return new WaitForSeconds(transition.sceneTriggerDelay);

        if(sceneToTransitionTo != string.Empty)
        {
            SwitchScene(sceneToTransitionTo);
        }
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TransitionToRoomInScene(Transform targetPlayerTransform)
    {
        playerTransform.position = targetPlayerTransform.position;
    }
}
