#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;
using OverlayFormTemplate.Utils;
using ReactiveUI;

#endregion

namespace OverlayFormTemplate.ViewModels;

public class ViewModelBase : ReactiveObject, IDisposable {
    private readonly List<IDisposable> _disposables = new();
    private bool _isBusy;

    public bool IsBusy {
        get => this._isBusy;
        set => this.RaiseAndSetIfChanged(ref this._isBusy, value);
    }

    public virtual void Dispose() {
        foreach (var d in this._disposables) {
            d?.Dispose();
        }
    }

    public void SubscribeTo<R>(Expression<Func<ViewModelBase, R>> prop, Action<ViewModelBase> onChanged) {
        var b = (MemberExpression)prop.Body;
        this.Subscribe(b.Member.Name, onChanged).Then(this.MarkForCleanup);
    }

    protected ICommand CreateCommand(Func<Task> doThis) =>
        ReactiveCommand.Create(async () => {
            try {
                await doThis();
            }
            catch (Exception exc) {
            }
        });

    protected ICommand CreateCommand(Action doThis) =>
        ReactiveCommand.Create(() => {
            try {
                doThis();
            }
            catch (Exception exc) {
            }
        });

    protected void MarkForCleanup(IDisposable d) => this._disposables.Add(d);

    protected void Exec(Action a) {
        try {
            a();
        }
        catch (Exception e) {
        }
    }

    protected async Task Exec(Func<Task> a) {
        try {
            await a();
        }
        catch (Exception e) {
        }
    }
}