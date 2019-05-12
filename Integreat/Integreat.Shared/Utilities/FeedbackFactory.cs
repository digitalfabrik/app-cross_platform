using Integreat.Shared.Models.Feedback;

namespace Integreat.Shared.Utilities
{
    public class FeedbackFactory
    {

        public IFeedback GetFeedback(FeedbackType type, FeedbackKind kindOfFeedback, string comment, int? pageId = null, string extraString = "")
        {
            switch (type)
            {
                case FeedbackType.Page:
                    return new FeedbackPage
                    {
                        Id = pageId,
                        Permalink = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                case FeedbackType.Extra:
                    return new FeedbackExtra
                    {
                        Alias = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                case FeedbackType.Search:
                    return new FeedbackSearch
                    {
                        Query = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                default:
                    return new FeedbackGeneral
                    {
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
            }
        }
    }
}
