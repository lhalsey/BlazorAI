using BlazorAI.Shared.Types;
using System.Collections.Generic;
using System.Threading;

namespace BlazorAI.Client.Pages
{
    public interface ISolverPage<TResult>
    {
        SolverParameters DefaultParameters { get; }

        IAsyncEnumerable<Result<TResult>> GetResults(
            CancellationTokenSource token,
            SolverParameters parameters);

        void HandleResult(Result<TResult> result);
    }
}
