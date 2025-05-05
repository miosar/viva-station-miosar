using Content.Server._Viva.GameTicking.Rules.Components;
using Content.Server.Antag;
using Content.Server.GameTicking.Rules;
using Content.Server.Mind;

namespace Content.Server._Viva.GameTicking.Rules;

public sealed class CCDAgentRuleSystem : GameRuleSystem<CCDAgentRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly MindSystem _mindSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CCDAgentRuleComponent, AfterAntagEntitySelectedEvent>(AfterSelected);
    }

    private void AfterSelected(Entity<CCDAgentRuleComponent> ent, ref AfterAntagEntitySelectedEvent args)
    {
        MakeCCDAgent(args.EntityUid, ent);
    }

    private void MakeCCDAgent(EntityUid agent, CCDAgentRuleComponent component)
    {
        var briefing = Loc.GetString("ccd-role-greeting");
        _antag.SendBriefing(agent, briefing, null, null);
    }
}
