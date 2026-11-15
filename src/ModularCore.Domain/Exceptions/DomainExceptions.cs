namespace ModularCore.Domain.Exceptions;
public class NotFoundException : Exception { public NotFoundException(string message) : base(message) { } }
public class InvalidStatusTransitionException : Exception { public InvalidStatusTransitionException(string message) : base(message) { } }
public class DomainValidationException : Exception { public DomainValidationException(string message) : base(message) { } }