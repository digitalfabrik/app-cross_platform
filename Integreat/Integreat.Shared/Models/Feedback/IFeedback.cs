namespace Integreat.Shared.Models.Feedback
{
    public interface IFeedback
    {
        string Comment { get; set; }
        string Rating { get; set; }
    }
}
