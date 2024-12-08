#region
#endregion

using OverlayFormTemplate.Messages;
using ReactiveUI;
using System.Windows.Input;

namespace OverlayFormTemplate.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _enteredName = string.Empty;
    private string _enteredLastName = string.Empty;

    public MainWindowViewModel()
    {
        this.OpenOverlayCommand = this.CreateCommand(this.OpenOverlay);

        MessagingSystem.MessageBus.Subscribe<OverlayFormDataMessage>(this.OnOverlayFormSubmit);
    }

    public OverlayHostViewModel Overlay { get; } = new();

    public string SubTitle { get; } = "Overlay PopUp Example";
    public string Title { get; } = "Overlay PopUp with Messaging System";

    public string EnteredName
    {
        get => this._enteredName;
        set => this.RaiseAndSetIfChanged(ref this._enteredName, value);
    }

    public string EnteredLastName
    {
        get => this._enteredLastName;
        set => this.RaiseAndSetIfChanged(ref this._enteredLastName, value);
    }


    private void OnOverlayFormSubmit(OverlayFormDataMessage obj) =>
    this.Exec(() =>
    {
        this.EnteredLastName = obj.LastName;
        this.EnteredName = obj.FirstName;
    });

    public ICommand OpenOverlayCommand { get; }
    public void OpenOverlay()
    {
        OverlayViewModel overlayVm = new();

        OpenOverlayMessage msg = new(overlayVm);

        MessagingSystem.MessageBus.Publish(msg);
    }
}