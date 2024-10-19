using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DiceHolder : MonoBehaviour
{
    public ChallengeGoalDefinition spec;

    public GameObject highlight;
    public TMP_Text target;
    public SpriteRenderer side;
    
    public int maxHold;

    public int goalValue;
    public int accumulatedValue;

    public SpriteRenderer checkmark;
    public ChallengeContainer owner;
    
    DiceZone zone;

    void Start()
    {
        zone = GetComponent<DiceZone>();
        G.main.OnReleaseDrag += TryClaim;
    }

    void TryClaim(InteractiveObject arg0)
    {
        if (IsDiceEntrapped(arg0) && !arg0.state.isClaimed && !isComplete && arg0.IsPlayedOrWildcard())
        {
            arg0.state.isClaimed = true;
            ClaimDiceIntoGoal(arg0).Forget();
        }
    }

    async UniTaskVoid ClaimDiceIntoGoal(InteractiveObject arg0)
    {
        owner.transform.DOShakePosition(0.2f, 0.2f, 50);
        G.audio.Play<SFX_Impact>();

        zone.Claim(arg0);
        var inters = G.main.interactor.FindAll<IOnPutIntoGoal>();
        foreach (var i in inters)
            await i.OnGoalDice(arg0.state, this);

        await UniTask.WaitForSeconds(0.2f);

        if (spec.type == GoalType.SINK)
            isComplete = accumulatedValue >= spec.goalValue;
        else
            isComplete = true;

        if (isComplete)
        {
            G.feel.UIPunchSoft();
            await G.main.KillDice(arg0.state);
        }

        await G.main.ClearUpChallenges();
        await G.main.CheckForWin();
    }

    bool IsDiceEntrapped(InteractiveObject arg0)
    {
        if (arg0 == null)
            return false;

        var isInRange = Vector2.Distance(arg0.transform.position, transform.position) < 1f;
        if (!isInRange)
            return false;

        if (!GoalMatcher.Matches(spec, arg0))
            return false;

        if (!arg0.IsPlayedOrWildcard())
            return false;
        
        return true;
    }

    void Update()
    {
        RenderTarget();
        HighlightLogic();
    }

    void HighlightLogic()
    {
        highlight.SetActive(!isComplete);
        
        var h = highlight.GetComponent<SpriteRenderer>();
        if (G.drag_dice != null && GoalMatcher.Matches(spec, G.drag_dice))
        {
            if (IsDiceEntrapped(G.drag_dice))
                h.color = new Color(1f, 1f, 1f, 1f);
            else
                h.color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            h.color = new Color(1f, 1f, 1f, 0f);
        }
        
    }

    void RenderTarget()
    {
        side.enabled = false;
        
        switch (spec.type)
        {
            case GoalType.NONE:
                target.text = "";
                break;
            case GoalType.VALUE:
                side.enabled = true;
                side.sprite = G.main.sides[spec.goalValue-1];
                target.text = "";
                break;
            case GoalType.GREATER_THAN:
                target.text = "> "+spec.goalValue.ToString();
                break;
            case GoalType.LESS_THAN:
                target.text = "< "+spec.goalValue.ToString();
                break;
            case GoalType.EVEN:
                target.text = "EVEN";
                break;
            case GoalType.ODD:
                target.text = "ODD";
                break;
            case GoalType.BLOCK:
                target.text = "BLOCK";
                break;
            case GoalType.ANY:
                target.text = "ANY";
                break;
            case GoalType.SINK:
                target.text = Mathf.Max(0, spec.goalValue-accumulatedValue).ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (IsFilled())
        {
            target.text = "";
            side.enabled = false;
            checkmark.gameObject.SetActive(true);
        }
    }

    bool isComplete;

    public bool IsFilled()
    {
        return isComplete;
    }
}

public static class GoalMatcher
{
    public static bool Matches(ChallengeGoalDefinition goal, InteractiveObject obj)
    {
        if (obj == null) return false;
        if (obj.state == null) return false;
        if (!obj.IsPlayedOrWildcard()) return false;
        if (obj.state.model.Is<TagWildcard>()) return true;
        
        Debug.Log(goal.type);
        switch (goal.type)
        {
            case GoalType.NONE: return true;
            
            case GoalType.VALUE: return obj.state.rollValue == goal.goalValue;
            
            case GoalType.GREATER_THAN:return obj.state.rollValue > goal.goalValue;
            
            case GoalType.LESS_THAN:return obj.state.rollValue < goal.goalValue;
            
            case GoalType.EVEN: return obj.state.rollValue % 2 == 0;
            case GoalType.ODD: return obj.state.rollValue % 2 != 0;
            
            case GoalType.BLOCK: throw new NotImplementedException(); break;
            
            case GoalType.ANY: return obj != null;
            
            case GoalType.SINK: return obj.state.isPlayed;
            
            default: throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}