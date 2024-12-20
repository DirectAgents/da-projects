﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl
{
    public abstract class Extracter<T> : IDisposable
    {
        private const int DefaultCollectionBoundedCapacity = 5000;

        private readonly object locker = new object();
        private readonly int collectionBoundedCapacity;

        private BlockingCollection<T> items;
        private int added;

        public event Action<FailedEtlException> ProcessEtlFailedWithoutInformation;

        protected Extracter(int collectionBoundedCapacity = DefaultCollectionBoundedCapacity)
        {
            this.collectionBoundedCapacity = collectionBoundedCapacity;
            Initialize();
        }

        public Thread Start()
        {
            var thread = new Thread(ExtractWithCatch);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return thread;
        }

        public void Initialize()
        {
            items = new BlockingCollection<T>(collectionBoundedCapacity);
            lock (locker)
            {
                added = 0;
            }
        }

        public void End()
        {
            items.CompleteAdding();
        }

        public bool Done => items.IsAddingCompleted;

        public IEnumerable<T> EnumerateAll()
        {
            return items.GetConsumingEnumerable();
        }

        public int Count => items.Count;

        public int Added
        {
            get
            {
                lock (locker)
                {
                    return added;
                }
            }
        }

        protected void Add(IEnumerable<T> extracted)
        {
            foreach (var item in extracted)
            {
                items.Add(item);
                lock (locker)
                {
                    added++;
                }
            }
        }

        protected void Add(T item)
        {
            Add(new List<T> { item });
        }

        private void ExtractWithCatch()
        {
            try
            {
                Extract();
            }
            catch (Exception e)
            {
                var exception = new FailedEtlException(null, null, null, e);
                LogExceptionInExtractor(exception);
                if (ProcessEtlFailedWithoutInformation == null)
                {
                    CommandExecutionContext.Current.MarkCurrentExecutionAsFailed();
                }
                else
                {
                    ProcessEtlFailedWithoutInformation.Invoke(exception);
                }
                End();
            }
        }

        /// <summary>
        /// The derived class implements this method, which calls Add() for each item
        /// extracted and then calls End() when complete.
        /// </summary>
        protected abstract void Extract();

        // http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        // Implement IDisposable. 
        // Do not make this method virtual. 
        // A derived class should not be able to override this method. 
        public void Dispose()
        {
            Dispose(true);
            // http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
            // This object will be cleaned up by the Dispose method. 
            // Therefore, you should call GC.SupressFinalize to 
            // take this object off the finalization queue 
            // and prevent finalization code for this object 
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        // http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
        // Dispose(bool disposing) executes in two distinct scenarios. 
        // If disposing equals true, the method has been called directly 
        // or indirectly by a user's code. Managed and unmanaged resources 
        // can be disposed. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            // If disposing equals false, the method has been called by the 
            // runtime from inside the finalizer and you should not reference 
            // other objects. Only unmanaged resources can be disposed. 
            if (disposing)
            {
                // Dispose managed resource
                items.Dispose();
            }

            // Dispose unmanaged resource
            disposed = true;
        }

        private void LogExceptionInExtractor(FailedEtlException exc)
        {
            var exception = new Exception($"Exception in extractor: {exc}", exc);
            if (exc.AccountId.HasValue)
            {
                Logger.Error(exc.AccountId.Value, exception);
            }
            else
            {
                Logger.Error(exception);
            }
        }
    }
}
