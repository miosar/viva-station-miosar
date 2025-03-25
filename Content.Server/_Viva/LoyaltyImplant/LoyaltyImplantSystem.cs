

using Content.Server._Impstation.Thaven;
using Content.Shared._Impstation.Thaven.Components;
using Content.Shared.Dataset;
using Content.Shared.Implants;
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
    }

    private void OnImplant(EntityUid uid, LoyaltyImplantComponent component, ImplantImplantedEvent args)
    {
        if (args.Implanted == null)
            return;

         var target = args.Implanted.Value;

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
}
