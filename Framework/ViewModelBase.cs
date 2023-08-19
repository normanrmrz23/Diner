using System.Reactive.Disposables;
using System.Reactive.Linq;
using Serilog;

namespace Diner.Framework
{
    public class ViewModelBase : BindableBase, INavigatingAware, INavigatedAware, IDestructible, IEquatable<ViewModelBase>
    {
        private Guid Id { get; }
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public ViewModelBase()
        {
            Id = Guid.NewGuid();
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// A helper method to be able to run async/await methods in void methods that cannot be turned into Task methods.
        /// Don't just use this all over the place. The lifecycle methods like OnInitialize or Destroy are places that this
        /// method was intended for. 
        /// </summary>
        /// <param name="taskToRun"></param>
        /// <param name="logger"></param>
        protected void RunAsyncAwait(Task taskToRun, ILogger logger = null)
        {
            Observable.Create<object>(async observer =>
            {
                try
                {
                    await taskToRun;
                }
                catch (Exception e)
                {
                    logger?.Error($"Failed to run async task {e}");
                }

                observer.OnCompleted();
                return Disposable.Empty;
            }).Subscribe();
        }

        public async Task OnNavigatingToAsync(NavigationParameters parameters)
        {
         //   await Log.Logger.LogElapsedTimeAsync(() => OnInitializeAsync(parameters), nameof(OnInitializeAsync));
        }

        public async Task OnNavigatedToAsync(NavigationParameters parameters)
        {
            //await Log.Logger.LogElapsedTimeAsync(() => OnReadyAsync(parameters), nameof(OnReadyAsync));
        }

        public async Task OnNavigatedFromAsync()
        {
           // await Log.Logger.LogElapsedTimeAsync(OnNavigatedAwayAsync, nameof(OnNavigatedAwayAsync));
        }

        public virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnReadyAsync(NavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatedAwayAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnPagePoppedAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Destroy()
        {
            Log.Logger.Verbose($"ViewModel '{GetType()}' is being destroyed");

            Log.Verbose($"Destroying '{Title}', calling Dispose");
            Disposables?.Dispose();
        }

        protected virtual Task OnExecutingBackButtonAsync()
        {
            return Task.CompletedTask;
        }

        public virtual async Task<bool> HandleBackButtonAsync()
        {
            try
            {
                await OnExecutingBackButtonAsync();
                Destroy();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, $"ViewModel '{GetType()}' failure during Destroy()");
                return false;
            }
        }

        public bool Equals(ViewModelBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ViewModelBase)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}


