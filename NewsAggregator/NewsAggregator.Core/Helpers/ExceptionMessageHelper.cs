namespace NewsAggregator.Core.Helpers
{
    public static class ExceptionMessageHelper
    {
        public static string GetExceptionMessage(Exception ex)
        {
            return $"{DateTime.Now}: Exception in {ex.Source}, message: {ex.Message}, stacktrace: {ex.StackTrace}";
        }
    }
}
