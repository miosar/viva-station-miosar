

using Robust.Shared.GameStates;

namespace Content.Server._Viva.LoyaltyImplant;

[RegisterComponent, NetworkedComponent]
public sealed partial class LoyaltyImplantComponent : Component
{
    [DataField]
    public EntityUid? Target = null;
}
