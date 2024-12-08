#region

using ReactiveUI;

using MessagingSystem;
using OverlayFormTemplate.Messages;
using OverlayFormTemplate.Utils;

#endregion

namespace OverlayFormTemplate.ViewModels;

public class OverlayHostViewModel : ViewModelBase {
    private bool _canShowOverlay;

    private IOverlayViewModel? _content;

    public OverlayHostViewModel()
    {
        MessagingSystem.MessageBus.Subscribe<OpenOverlayMessage>(this.OnOpenOverlay).Then(this.MarkForCleanup);
        MessagingSystem.MessageBus.Subscribe<CloseOverlayMessage>(this.OnCloseOverlay).Then(this.MarkForCleanup);
    }

    public bool CanShowOverlay {
        get => this._canShowOverlay;
        set => this.RaiseAndSetIfChanged(ref this._canShowOverlay, value);
    }

    public IOverlayViewModel? Content {
        get => this._content;
        set => this.RaiseAndSetIfChanged(ref this._content, value);
    }

    private void OnCloseOverlay(CloseOverlayMessage obj) {
        this.CanShowOverlay = false;
        this.Content = null;
    }

    private void OnOpenOverlay(OpenOverlayMessage obj) {
        this.Content = obj.OverlayContent;
        this.CanShowOverlay = true;
    }
}