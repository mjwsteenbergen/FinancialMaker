using Windows.UI;

namespace FinancialMaker.Steps
{
    public interface IStep
    {
        string StepName { get; }
        Color StepColor { get; }
    }
}