using System;
namespace Integreat.Shared.Utilities
{
    public enum FeedbackType
    {
        [StringValue("")]
        Page = 1,
        [StringValue("extra")]
        Extra = 2,
        [StringValue("search")]
        Search = 3,
        [StringValue("cities")]
        Cities = 4,
        [StringValue("categories")]
        Categories = 5,
        [StringValue("extras")]
        Extras = 6,
        [StringValue("events")]
        Events = 7
    }

    public enum FeedbackKind
    {
        [StringValue("up")]
        Up = 1,
        [StringValue("down")]
        Down = 2
    }
}
