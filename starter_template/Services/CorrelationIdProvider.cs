using starter_template.Interfaces;

namespace starter_template.Services;

public class CorrelationIdProvider:ICorrelationIdProvider
{
    private string _correlationId = Guid.NewGuid().ToString();

    public string Get() => _correlationId;

    public void Set(string correlationId)
    {
        if (!string.IsNullOrWhiteSpace(correlationId))
        {
            _correlationId = correlationId;
        }
    }
}