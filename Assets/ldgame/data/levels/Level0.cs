using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Math;

public class Level0 : CMSEntity
{
    public Level0()
    {
        Define<TagExecuteScript>().toExecute = Script;
    }

    async UniTask Script()
    {
        G.main.HideHud();
        G.ui.click_to_continue.SetActive(true);
        
        await G.main.Say("A tiny pack of tiny creatures was lost in the woods...");
        await G.main.SmartWait(5f);
        G.ui.click_to_continue.SetActive(false);
        
        await G.main.Say("The food was scarce...");
        await G.main.SmartWait(3f);
        await G.main.Say("They had to choose who will be left behind...");
        await G.main.SmartWait(3f);
        await G.main.Unsay();

        G.main.AdjustSay(-1.2f);
        await G.main.Say("Pick 2, choose wisely.");
        
        G.ui.EnableInput();
        await G.main.SetupPicker(new List<DiceRarity>() { DiceRarity.COMMON, DiceRarity.UNCOMMON }, 2, dontClear: true);
        G.ui.DisableInput();
        
        await G.main.Say($"Poor <b>{G.main.picker.objects[0].GetNme()}</b> was <b>left behind</b>.");

        await G.main.SmartWait(3f);
        
        await G.main.picker.Clear();
    }
}

public class TagExecuteScript : EntityComponentDefinition
{
    public Func<UniTask> toExecute;
}