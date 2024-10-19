using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MorphingDice : DiceBase
{
    public MorphingDice()
    {
        Define<TagDescription>().loc = $"If rolls greater than the dice in {TextStuff.Front} - turns into a wolf";
        Define<TagRarity>().rarity = DiceRarity.COMMON;

        Define<TagMorph>().id = E.Id<PlusOneGrowDice>();

        Define<TagAnimalView>().name = "Sheep";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "sheep");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagMorph : EntityComponentDefinition
{
    public string id;
}

public class MorphInteraction : BaseInteraction, IOnPlay
{
    public async UniTask OnPlayDice(DiceState dice)
    {
        if (dice.model.Is<TagMorph>(out var tm))
        {
            var nextDice = G.main.field.GetNextDice(dice.view);
            if (nextDice != null && dice.rollValue > nextDice.state.rollValue)
            {
                var interactiveObject = G.main.CreateDice(tm.id);
                interactiveObject.transform.position = dice.view.transform.position;
                interactiveObject.moveable.targetPosition = interactiveObject.transform.position;
                
                await G.main.KillDice(dice);
                await G.main.PlayDice(interactiveObject);
            }
        }
    }
}