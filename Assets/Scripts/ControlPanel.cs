using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool revealed = false;
    private const float hiddenPos = 105f;
    private const float revealedPos = -100f;
    private float pos;

    void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        pos = hiddenPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (revealed) {
            revealPanel();
        }
        else {
            hidePanel();
        }
        rectTransform.anchoredPosition = new Vector2(pos, 0);
    }

    public void togglePanel(Image revealBtnImage) {
        if (revealed) {
            revealBtnImage.rectTransform.rotation = Quaternion.Euler(0,0,270);

        }
        else {
            revealBtnImage.rectTransform.rotation = Quaternion.Euler(0,0,90);
        }
        revealed = !revealed;

    }

    private void revealPanel() {
        if (pos > revealedPos) {
            pos -= 400 * Time.deltaTime;
        }
        else {
            pos = revealedPos;
        }
    }

    private void hidePanel() {
        if (pos < hiddenPos) {
            pos += 400 * Time.deltaTime;
        }
        else {
            pos = hiddenPos;
        }
    }
}
