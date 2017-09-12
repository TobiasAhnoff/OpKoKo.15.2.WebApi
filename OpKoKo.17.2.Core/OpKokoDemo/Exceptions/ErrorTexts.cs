namespace OpKokoDemo.Exceptions
{
    public static class ErrorTexts
    {
        public const string ValidationErrorCode = "ValidationError";
        public const string ResourceLockedExceptionCode = "Resource_Locked";
        public const string ResourceLockedExceptionMessage = "The resource requested is currently locked for modification. Try again.";
        public const string UnhandledExceptionCode = "UnhandledException";
        public const string UnhandledExceptionMessage = "An unhandled exception occured. Please contact your Super Admin!";
        public const string ServiceUnavailableCode = "Service_Unavailable";
        public const string ServiceUnavailableMessage = "One or more of the underlying services are unavailable";
        public const string BadRequestErrorCode = "BadRequest";
        public const string BadRequestErrorMessage = "The request could not be understood by the server due to malformed syntax.";
        public const string ConflictErrorCode = "Conflict";
        public const string ConflictErrorMessage = "The request could not be completed due to a conflict with the current state of the target resource";
        public const string NotFoundErrorCode = "NotFound";
        public const string NotFoundErrorCodeMessage = "One or more of the underlying resources could not be found";
    }
}
