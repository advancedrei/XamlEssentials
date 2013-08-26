using System;
using System.Threading.Tasks;

namespace Microsoft.Phone.Maps.Services
{
    
    public static class QueryExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <remarks>
        /// Technique from: http://compiledexperience.com/blog/posts/async-geocode-query
        /// </remarks>
        public static Task<T> ExecuteAsync<T>(this Query<T> query)
        {

            var taskSource = new TaskCompletionSource<T>();
            EventHandler<QueryCompletedEventArgs<T>> handler = null;

            handler = (s, e) =>
            {
                query.QueryCompleted -= handler;

                if (e.Cancelled)
                    taskSource.SetCanceled();
                else if (e.Error != null)
                    taskSource.SetException(e.Error);
                else
                    taskSource.SetResult(e.Result);
            };

            query.QueryCompleted += handler;

            query.QueryAsync();

            return taskSource.Task;
        }


    }
}
