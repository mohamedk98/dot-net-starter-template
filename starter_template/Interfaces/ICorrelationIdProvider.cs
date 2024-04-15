namespace starter_template.Interfaces;

public interface ICorrelationIdProvider
{
    string Get();
    void Set(string correlationId);
}