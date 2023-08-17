using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Serilog;

namespace Diner.Persistence
{
    public interface IRepository<T>
    {
        IObservable<T> GetAll();
        IObservable<T> Get(Guid id);
        T? GetOne(Guid id);
        IObservable<Unit> Save(T input);
        IObservable<Unit> Delete(Guid id);
        IObservable<Unit> DeleteAll();
        IObservable<Guid> GetIds();
        void Flush();
    }

    public class MonkeyCacheRepository<T> : IRepository<T> where T : IHasIdentifier
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        private readonly object _syncLock = new();
        private readonly Func<Guid, string> _idKeyFunc;
        private readonly string _indexKey;
        private readonly Type _repoType;

        public MonkeyCacheRepository(IDataService dataService, ILogger logger)
        {
            _dataService = dataService;
            _logger = logger;

            _repoType = typeof(T);
            _indexKey = $"{_repoType.Name}-index";
            _idKeyFunc = id => $"{_repoType.Name}-id-{id.ToString()}";
        }

        public IObservable<T> GetAll()
        {
            return GetAllEnum().ToObservable();
        }

        public IEnumerable<T> GetAllEnum()
        {
            var idKeys = GetIndex().ToList();
            _logger.Debug("Getting all {count} of {type}", idKeys.Count, _repoType);

            return idKeys.Select(GetByKey);
        }

        public IObservable<T> Get(Guid id)
        {
            var result = GetOne(id);

            return result == null ? Observable.Empty<T>() : Observable.Return(result);
        }

        public IObservable<Unit> Save(T input)
        {
            var assignmentKey = _idKeyFunc(input.Id);
            return Transaction(() =>
            {
                AddToIndex(assignmentKey);
                _dataService.InsertObject(assignmentKey, input);
                _logger.Verbose("Saved {type} with id {id}", _repoType, input.Id);
                return Observable.Return(Unit.Default);
            });
        }

        public IObservable<Unit> Delete(Guid id)
        {
            var assignmentKey = _idKeyFunc(id);

            return Transaction(() =>
            {
                RemoveFromIndex(assignmentKey);
                _dataService.RemoveObject(assignmentKey);
                _logger.Verbose("Deleted {type} with id {id}", _repoType, id);
                return Observable.Return(Unit.Default);
            });
        }

        public IObservable<Unit> DeleteAll()
        {
            return Transaction(() =>
            {
                var idKeys = GetIndex().ToList();
                foreach (var id in idKeys)
                {
                    _dataService.RemoveObject(id);
                }
                SetIndex(Enumerable.Empty<string>());
                _logger.Verbose("Deleted all {count} of {type}", _repoType, idKeys.Count);
                return Observable.Return(Unit.Default);
            });
        }

        public IObservable<Guid> GetIds()
        {
            const int guidStringLength = 36;
            _logger.Verbose("Getting ids of {type}", _repoType);

            return GetIndex().Select(idString =>
            {
                var startIndex = idString.Length - guidStringLength;
                var guid = idString.Substring(startIndex);
                return Guid.Parse(guid);
            }).ToObservable();
        }

        public void Flush() { }

        private IEnumerable<string> GetIndex()
        {
            return _dataService.Get<string[]>(_indexKey) ?? Enumerable.Empty<string>();
        }

        private void AddToIndex(string assignmentKey)
        {
            var ids = GetIndex();
            var newIndex = ids.Union(new[] { assignmentKey });
            SetIndex(newIndex);
        }

        private void SetIndex(IEnumerable<string> ids)
        {
            _dataService.InsertObject(_indexKey, ids.ToArray());
        }

        private void RemoveFromIndex(string assignmentKey)
        {
            var ids = GetIndex();
            var newIndex = ids.Except(new[] { assignmentKey });
            SetIndex(newIndex);
        }

        private T? GetByKey(string key)
        {
            return _dataService.Get<T>(key);
        }

        public T? GetOne(Guid id)
        {
            var assignmentKey = _idKeyFunc(id);
            _logger.Verbose("Getting {type} id {id}.", _repoType, id);
            var result = GetByKey(assignmentKey);
            return result;
        }

        private TOutput Transaction<TOutput>(Func<TOutput> func)
        {
            lock (_syncLock)
            {
                return func();
            }
        }
    }
}
