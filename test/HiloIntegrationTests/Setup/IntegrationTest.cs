namespace HiloIntegrationTests.Setup;

public class IntegrationTestAttribute : FactAttribute
{
    public IntegrationTestAttribute()
    {
        var envVarValue = Environment.GetEnvironmentVariable("INTEGRATIONTESTS") ?? "";
        if (!envVarValue.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Skip = "Integration tests should only run in docker-compose, by setting INTEGRATIONTESTS to true";
        }
    }
}