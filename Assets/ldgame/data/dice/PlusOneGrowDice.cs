using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlusOneGrowDice : DiceBase
{
    public PlusOneGrowDice()
    {
        Define<TagDescription>().loc = $"Increases it's value by 1 each turn WHEN it was played. \n (Is not removed from board on end turn)";
        Define<TagRarity>().rarity = DiceRarity.RARE;

        Define<TagGrowEachTurn>().delta = 1;
        Define<TagThriving>();
        
        Define<TagAnimalView>().name = "Wolf";
        Define<TagAnimalView>().sprite = SpriteUtil.Load("animals", "wolf");
        Define<TagAnimalView>().color = Color.white;
    }
}

public class TagThriving : EntityComponentDefinition
{
}

public class TagGrowEachTurn : EntityComponentDefinition
{
    public int delta;
}

public class GrowEachTurn : BaseInteraction, IOnEndTurnFieldDice
{
    public async UniTask OnEndTurnInField(DiceState state)
    {
        if (state.model.Is<TagGrowEachTurn>(out var eg))
        {
            await state.view.SetValue(state.rollValue + eg.delta);
        }
    }
}