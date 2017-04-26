using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inchvi.IO.Util
{
    public static class TaskHelpers
    {
        public static bool WaitOne(this WaitHandle waitHandle, int millisecondsTimeout, CancellationToken cancellationToken)
        {
            var handleArray = new[] { waitHandle, cancellationToken.WaitHandle };
            var finishedId = WaitHandle.WaitAny(handleArray, millisecondsTimeout);
            if (finishedId == Array.IndexOf(handleArray, cancellationToken.WaitHandle))
            {
                //If token wait handle was first to signal then exit
                throw new OperationCanceledException("Cancellation Requested");
            }
            if (finishedId == Array.IndexOf(handleArray, waitHandle))
            {
                return true;
            }
            return false;
        }
        public static Task CancellableSyncMethod(Action<CancellationToken> syncAction, CancellationTokenSource cts)
        {
            var outer = Task.Factory.StartNew(() =>
            {
                //Start the synchronous method - passing it a cancellation token
                var inner = Task.Factory.StartNew(_ =>
                {
                    syncAction(cts.Token);
                }, cts.Token);
                inner.Wait(cts.Token);

            }, cts.Token);

            return outer;
        }

    }
}