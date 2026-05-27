namespace HotelBookingSystem.Domain.Services;

public class RetryPolicyService
{
    public void Execute(Action action, int retryCount = 3)
    {
        int attempts = 0;

        while (attempts < retryCount)
        {
            try
            {
                action();
                return;
            }
            catch
            {
                attempts++;

                Console.WriteLine($"Retry attempt: {attempts}");

                Thread.Sleep(1000);
            }
        }

        throw new Exception("Operation failed after retries.");
    }
}