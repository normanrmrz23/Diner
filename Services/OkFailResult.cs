namespace Diner.Services
{
    public readonly struct OkFailResult
    {
        public bool Success { get; }
        public bool Failed => !Success;
        public string Message { get; }

        public OkFailResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static OkFailResult Fail(string message)
        {
            return new OkFailResult(false, message);
        }

        public static OkFailResult Ok()
        {
            return new OkFailResult(true, string.Empty);
        }
    }
}
