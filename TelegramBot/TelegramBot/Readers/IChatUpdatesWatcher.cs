using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TelegramBot.Readers
{
    public interface IChatUpdatesWatcher<T> where T : class
    {
        event Action<T> MessageIsArrived;

        void StartWatch(CancellationToken cancellationToken);
    }
}
