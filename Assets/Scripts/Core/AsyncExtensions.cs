using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace MilehighWorld.Core
{
    public static class AsyncExtensions
    {
        public static Task WithCancellation(this UnityWebRequestAsyncOperation asyncOp, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (cancellationToken.IsCancellationRequested)
            {
                tcs.SetCanceled();
                return tcs.Task;
            }

            CancellationTokenRegistration registration = default;

            asyncOp.completed += _ =>
            {
                registration.Dispose();
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    tcs.TrySetResult(true);
                }
            };

            registration = cancellationToken.Register(() =>
            {
                asyncOp.webRequest.Abort();
                tcs.TrySetCanceled();
            });

            return tcs.Task;
        }
    }
}
