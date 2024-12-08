using System.Threading.Tasks;
using System.Windows.Input;
using OverlayFormTemplate.Messages;
using ReactiveUI;

namespace OverlayFormTemplate.ViewModels;

public class OverlayViewModel : ViewModelBase, IOverlayViewModel
{
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;

    public OverlayViewModel()
    {
        this.CancelCommand = this.CreateCommand(this.Close);
        this.OkayCommand = this.CreateCommand(this.OnOkay);
        this.Title = "Overlay Form Sample";
    }

    public string Title { get; }

    public string FirstName
    {
        get => this._firstName;
        set => this.RaiseAndSetIfChanged(ref this._firstName, value);
    }

    public string LastName
    {
        get => this._lastName;
        set => this.RaiseAndSetIfChanged(ref this._lastName, value);
    }

    public ICommand CancelCommand { get; }
    public void Close() => MessagingSystem.MessageBus.Publish(new CloseOverlayMessage(this));

    public ICommand OkayCommand { get; }
    private async Task OnOkay()
    {
        MessagingSystem.MessageBus.Publish(new OverlayFormDataMessage(this._firstName, _lastName));
        this.Close();
    }
}