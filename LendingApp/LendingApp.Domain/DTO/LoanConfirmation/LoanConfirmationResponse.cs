namespace LendingApp.Domain.DTO.LoanConfirmation
{
    public class LoanConfirmationResponse
    {
        public bool Success { get; set; }

        public string ConfirmationNumber { get; set; } = string.Empty;

        public string ApplicationId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public List<string>? NextSteps { get; set; }

        public string? EstimatedProcessingTime { get; set; }
    }
}
