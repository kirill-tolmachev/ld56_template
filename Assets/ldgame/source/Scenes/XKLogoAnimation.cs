using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XKLogoAnimation : MonoBehaviour
{
    public TMP_Text text;
    public SpriteRenderer logo;
    bool skip;

    async void Start()
    {
        G.audio.Play<Sound_AmbienceForest>();
        
        if (GameSettings.SKIP_INTRO)
        {
            SceneManager.LoadScene(GameSettings.MAIN_SCENE);
        }

        text.alpha = 0f;

        text.text = "GAME BY XK";

        await SmartWait(0.5f);

        G.audio.Play<SFX_Click>();
        G.feel.UIPunchSoft();

        text.DOFade(1f, 0.5f);

        await SmartWait(1.5f);

        G.audio.Play<SFX_Click>();
        G.feel.UIPunchSoft();

        text.DOFade(0f, 1f);

        await SmartWait(1f);

        text.text = "<size=50%>MADE IN 48 HOURS FOR\nLUDUM DARE 56</size>";

        G.audio.Play<SFX_Click>();
        G.feel.UIPunchSoft();

        text.DOFade(1f, 0.5f);

        await SmartWait(3.5f);

        G.audio.Play<SFX_Click>();
        G.feel.UIPunchSoft();

        text.DOFade(0f, 1.5f);

        await SmartWait(1.5f);

        G.audio.Play<SFX_Click>();
        G.feel.UIPunchSoft();

        logo.DOFade(1f, 1f);

        await SmartWait(3f);

        logo.DOFade(0f, 2f);

        await SmartWait(1f);

        G.fader.FadeIn();
        await SmartWait(1f);

        SceneManager.LoadScene(GameSettings.MAIN_SCENE);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            skip = true;
        }
    }

    public async UniTask SmartWait(float f)
    {
        skip = false;
        while (f > 0 && !skip)
        {
            f -= Time.deltaTime;
            await UniTask.WaitForEndOfFrame();
        }
    }
}