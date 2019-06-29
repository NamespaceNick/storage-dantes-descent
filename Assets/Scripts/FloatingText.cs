using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// general purpose script for showing text above a 2D object
// things you can do to the text: bounce, fade out, appear on collision with object
public class FloatingText : MonoBehaviour {

    // TODO: implement BounceText() script
    public float bounceOffset = 0.1f;
    public float bounceIncrement = 0.001f;
    public float fadeInTime = 0.5f;
    public float fadeOutTime = 0.5f;
    [TextArea]
	public string message = "<Message goes here>";
    public string collisionTag = "Player";  // tag of object that triggers message reveal
    public bool persistentMessage = false; // is message always showing, or only when collided with?
    public bool textBounce = false;
    public bool fadeOut = false; // should message begin to fade out after it is created?
    public bool destroyAfterFade = false;
    public Text textBox;

    Vector3 origTextPosition;
    Coroutine textBounceCR, fadeOutCR;

    // note: this script prioritizes characteristics in the following order: fadeOut > persistentMessage > textBounce
	void Start()
    {
        Debug.Assert(textBox != null);
        textBox.enabled = true;
        message = textBox.text;
        // set whether text is visible on scene start
        textBox.CrossFadeAlpha((fadeOut || persistentMessage) ? 1f : 0f, 0f, true);
        origTextPosition = textBox.rectTransform.position;
        fadeOutCR = (fadeOut) ? StartCoroutine(FadeOut(fadeOutTime)) : null;
        textBounceCR = (textBounce) ? StartCoroutine(BounceText()) : null;
	}

    // reveal message when player gets near sign
	void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag(collisionTag) || persistentMessage || fadeOut) return;
        textBox.CrossFadeAlpha(1f, fadeInTime, false);
    }

    // hide message when player leaves sign, sets text position to default
    void OnTriggerExit2D(Collider2D collider)
    {
        if ((!collider.CompareTag(collisionTag)) || persistentMessage || fadeOut) return;
        textBox.CrossFadeAlpha(0f, fadeInTime, false);
	}

    // message bounces vertically
    // TODO: make bounce juicier by using LerpUnclamped with animation curves
    IEnumerator BounceText()
    {
        float yUpper = origTextPosition.y + bounceOffset;
        float yLower = origTextPosition.y - bounceOffset;
        Vector3 newPosition = textBox.rectTransform.position;
        while (true)
        {
            while (textBox.rectTransform.position.y < (yUpper))
            {
                newPosition.y += bounceIncrement;
                textBox.rectTransform.position = newPosition;
                yield return null;
            }
            while (textBox.rectTransform.position.y > (yLower))
            {
                newPosition.y -= bounceIncrement;
                textBox.rectTransform.position = newPosition;
                yield return null;
            }
            yield return null;
        }
    }

    // message fades out over fadeTime seconds, optionally destroys text object
    IEnumerator FadeOut(float fadeTime)
    {
        textBox.CrossFadeAlpha(0, fadeTime, false);
        if (destroyAfterFade)
        {
            yield return new WaitForSeconds(fadeTime);
            Destroy(textBox.gameObject);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}












