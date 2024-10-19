using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Plus1ToAllDice : DiceBase
{
    public Plus1ToAllDice()
    {
        Define<TagClone>();
        Define<TagDescription>().loc = $"Gives +1 to EVERY dice on the board.";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagFudgeAllDice>().delta = 1;

        Define<TagAnimalView>().name = "Panda";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "panda");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagFudgeAllDice : EntityComponentDefinition
{
    public int delta;
}

public class FudgeAllDiceInteraction : BaseInteraction, IOnPlay
{
    public async UniTask OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeAllDice>(out var tfl))
        {
            foreach (var lastDice in G.main.field.objects)
            {
                if (lastDice.state != dice)
                {
                    await lastDice.SetValue(lastDice.state.rollValue + tfl.delta);
                    lastDice.Punch();
                    await UniTask.WaitForSeconds(0.25f);
                }
            }
        }
    }
}