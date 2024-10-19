using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class TagListChallenges : EntityComponentDefinition
{
    public List<string> all = new List<string>();
}

public enum GoalType
{
    NONE,
    VALUE,
    GREATER_THAN,
    LESS_THAN,
    EVEN,
    ODD,
    BLOCK,
    ANY,
    SINK
}

public class ChallengeSinkCounter : BaseInteraction, IOnPutIntoGoal
{
    public async UniTask OnGoalDice(DiceState dice, DiceHolder holder)
    {
        if (holder.spec.type == GoalType.SINK)
        {
            await G.main.KillDice(dice);
            holder.accumulatedValue += dice.rollValue;
        }
    }
}

public class ChallengeGoalDefinition : EntityComponentDefinition
{
    public GoalType type;
    public int goalValue;
}

public class TagChallengeDefinition : EntityComponentDefinition
{
    public List<ChallengeGoalDefinition> goals = new List<ChallengeGoalDefinition>();
}

public class TagChallengePenalty : EntityComponentDefinition
{
    public int damage;
}

public class TagPreferSlot : EntityComponentDefinition
{
    public int idx;
}