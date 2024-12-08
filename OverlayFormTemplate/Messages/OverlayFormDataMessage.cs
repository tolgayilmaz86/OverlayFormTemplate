using MessagingSystem;

namespace OverlayFormTemplate.Messages;

public class OverlayFormDataMessage(string firstName, string lastName) : IMessage
{
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
}