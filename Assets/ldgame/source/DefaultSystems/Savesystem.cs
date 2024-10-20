using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Savefile
{
    public float volSfx = 0.8f;
    public float volMusic = 0.8f;
}

public class Savesystem : MonoBehaviour
{
    const string SlotId = "saveslot";
    public static Savefile slot;

    void Awake()
    {
        TryLoading();
        G.save = slot;

        SaveEverySoOften(destroyCancellationToken).Forget();
    }

    void TryLoading()
    {
        slot = JsonConvert.DeserializeObject<Savefile>(PlayerPrefs.GetString(SlotId, "{}"));
    }

    private async UniTask SaveEverySoOften(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            
            await UniTask.Delay(1000, cancellationToken: cancellationToken);
            Debug.Log("Save");
            var str = JsonConvert.SerializeObject(slot);
            PlayerPrefs.SetString(SlotId, str);
        }
    }
}