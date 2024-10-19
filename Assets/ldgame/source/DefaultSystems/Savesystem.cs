using System;
using System.Collections;
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

        SaveEverySoOften().Forget();
    }

    void TryLoading()
    {
        slot = JsonConvert.DeserializeObject<Savefile>(PlayerPrefs.GetString(SlotId, "{}"));
    }

    private async UniTask SaveEverySoOften()
    {
        while (true)
        {
            await UniTask.Delay(1000);
            
            var str = JsonConvert.SerializeObject(slot);
            PlayerPrefs.SetString(SlotId, str);
        }
    }
}