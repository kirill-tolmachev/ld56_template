using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FudgeDice : DiceBase
{
    public FudgeDice()
    {
        Define<TagTint>().color = Color.yellow;
        Define<TagFudgeDice>().delta = 1;
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Adds 1 to the dice in {TextStuff.Front} of it!";
        Define<TagRarity>().rarity = DiceRarity.COMMON;
        
        Define<TagAnimalView>().name = "Cat";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "cat");
        Define<TagAnimalView>().color = Color.white;
    }
}


public class TagFudgeDice : EntityComponentDefinition
{
    public int delta;
}

public class FudgeDiceInteraction : BaseInteraction, IOnPlay
{
    public async UniTask OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagFudgeDice>(out var tfl))
        {
            var lastDice = G.main.field.GetNextDice(dice.view);
            if (lastDice != null)
            {
                await lastDice.SetValue(lastDice.state.rollValue + tfl.delta);
                lastDice.Punch();
                await UniTask.WaitForSeconds(0.25f);
            }
        }
    }
}

public interface IOnPlay
{
    public UniTask OnPlayDice(DiceState dice);
}

public interface IOnPutIntoGoal
{
    public UniTask OnGoalDice(DiceState dice, DiceHolder holder);
}