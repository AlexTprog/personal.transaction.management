namespace personal.transaction.management.application.Common.Exceptions;

public class IdempotencyKeyReuseException(Guid requestId)
    : Exception($"The idempotency key '{requestId}' was already used with a different request payload.");
