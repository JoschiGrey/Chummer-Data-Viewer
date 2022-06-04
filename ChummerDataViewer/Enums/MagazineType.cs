using System.ComponentModel;

namespace ChummerDataViewer.Enums;

public enum MagazineType
{
    [Description("c")]
    Clip,
    [Description("cy")]
    Cylinder,
    [Description("belt")]
    Belt,
    [Description("ml")]
    MuzzleLoader,
    [Description("m")]
    Internal,
    [Description("b")]
    BreakAction,
    [Description("d")]
    Drum,
    [Description("")]
    Undefined,
    [Description("tank")]
    Tank,
    [Description("cb")]
    CabAndBall
}