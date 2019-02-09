using Integreat.Shared.Models.Feedback;

namespace Integreat.Shared.Utilities
{
    public class FeedbackFactory {

        public IFeedback GetFeedback(FeedbackType type, FeedbackKind kindOfFeedback, string comment, int? pageId = null, string extraString = "")
        {
            switch (type)
            {
                case FeedbackType.Page:
                    FeedbackPage fp = new FeedbackPage
                    {
                        Id = pageId,
                        Permalink = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                    return fp;
                case FeedbackType.Extra:
                    FeedbackExtra fe = new FeedbackExtra
                    {
                        Alias = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                    return fe;
                case FeedbackType.Search:
                    FeedbackSearch fs = new FeedbackSearch
                    {
                        Query = extraString,
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                    return fs;
                default:
                    FeedbackGeneral fg = new FeedbackGeneral
                    {
                        Comment = comment,
                        Rating = kindOfFeedback.GetStringValue()
                    };
                    return fg;
            }
        }
    }
}
