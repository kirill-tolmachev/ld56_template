using System.Collections;
using Cysharp.Threading.Tasks;
using Engine.Math;
using UnityEngine;

public class RerollDice : DiceBase
{
    public RerollDice()
    {
        Define<TagRerollerNext>();
        Define<TagDescription>().loc = $"{TextStuff.Fudge}: Rerolls the dice in {TextStuff.Front} of it.";
        Define<TagRarity>().rarity = DiceRarity.UNCOMMON;
        
        Define<TagAnimalView>().name = "Frog";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "frog");
        Define<TagAnimalView>().color = "#b9d161".ParseColor();
    }
}

public class TagRerollerNext : EntityComponentDefinition
{
    public int val;
}

public class RerollNext : BaseInteraction, IOnPlay
{
    public async UniTask OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagRerollerNext>())
        {
            var last = G.main.field.GetNextDice(dice.view);
            if (last != null)
            {
                dice.view.Punch();
                await UniTask.WaitForSeconds(0.25f);
                await G.main.Roll(last);
            }
        }
    }
}