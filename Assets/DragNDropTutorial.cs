using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DragNDropTutorial : MonoBehaviour
{
    public Sprite up;
    public Sprite dn;

    public SpriteRenderer spriteRen;

    private Transform fromt;
    private Transform tot;

    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        spriteRen.enabled = false;
    }

    public void Show(Transform from, Transform to)
    {
        fromt = from;
        tot = to;

        spriteRen.enabled = true;
        
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = new CancellationTokenSource();

        TutorializeDrag(cancellationTokenSource.Token).Forget();
    }

    private async UniTask TutorializeDrag(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            Debug.Log("from");
            spriteRen.transform.position = fromt.position;
            spriteRen.sprite = up;

            await UniTask.WaitForSeconds(0.25f, cancellationToken: cancellationToken);

            spriteRen.sprite = dn;

            await UniTask.WaitForSeconds(0.15f, cancellationToken: cancellationToken);

            await spriteRen.transform.DOMove(tot.position, 1f)
                .SetEase(Ease.Linear)
                .WithCancellation(cancellationToken);

            Debug.Log("to");

            await UniTask.WaitForSeconds(0.15f, cancellationToken: cancellationToken);

            spriteRen.sprite = up;

            await UniTask.WaitForSeconds(0.15f, cancellationToken: cancellationToken);
        }
    }

    public void Hide()
    {
        spriteRen.enabled = false;
        cancellationTokenSource?.Cancel();
        cancellationTokenSource = null;
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();
    }
}