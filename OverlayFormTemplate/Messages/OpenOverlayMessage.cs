#region

using MessagingSystem;
using OverlayFormTemplate.ViewModels;

#endregion

namespace OverlayFormTemplate.Messages;

public class OpenOverlayMessage(IOverlayViewModel overlayContent) : IMessage
{
    public IOverlayViewModel OverlayContent { get; } = overlayContent;
    public string Title { get; } = overlayContent.Title;
}