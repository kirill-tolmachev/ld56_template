using System.Collections;
using Cysharp.Threading.Tasks;

public class Level1 : CMSEntity
{
    public Level1()
    {
        Define<TagExecuteScript>().toExecute = Script;
        Define<TagListChallenges>().all.Add(E.Id<ChallengeRock>());
    }

    async UniTask Script()
    {
        G.main.AdjustSay(2f);
        await G.main.Say("A big rock was blocking their path...");
        await G.main.SmartWait(2f);
        await G.main.Unsay();
    }
}