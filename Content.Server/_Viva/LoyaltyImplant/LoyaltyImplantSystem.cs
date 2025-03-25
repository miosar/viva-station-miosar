

using Content.Server._Impstation.Thaven;
using Content.Shared._Impstation.Thaven.Components;
using Content.Shared.Dataset;
using Content.Shared.Implants;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Toolshed.TypeParsers;

namespace Content.Server._Viva.LoyaltyImplant;

public sealed class LoyaltyImplantSysten : EntitySystem
{
    [Dependency] private readonly ThavenMoodsSystem _moodSystem = default!;

    [ValidatePrototypeId<DatasetPrototype>]
    private const string LoyaltyImplantMoods = "LoyaltyImplantMoods";
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LoyaltyImplantComponent, ImplantImplantedEvent>(OnImplant);
        SubscribeLocalEvent<LoyaltyImplantComponent, EntGotRemovedFromContainerMessage>(OnRemove);
    }

    private void OnImplant(EntityUid uid, LoyaltyImplantComponent component, ImplantImplantedEvent args)
    {
        if (args.Implanted == null)
            return;

        var target = args.Implanted.Value;
        component.Target = target;

        if (args.Implanted != null)
        {
            EnsureComp<ThavenMoodsComponent>(target, out var moodComp);
            _moodSystem.ToggleEmaggable((target, moodComp));
            _moodSystem.ClearMoods((target, moodComp));
            _moodSystem.ToggleSharedMoods((target, moodComp));
            _moodSystem.TryAddRandomMood((target, moodComp), LoyaltyImplantMoods);
            Dirty(target, moodComp);
        }
    }

    private void OnRemove(EntityUid uid, LoyaltyImplantComponent component, EntGotRemovedFromContainerMessage args)
    {
        component.Target = args.Container.Owner;

        if (HasComp<ThavenMoodsComponent>(args.Container.Owner))
            RemComp<ThavenMoodsComponent>(args.Container.Owner);
    }
}
