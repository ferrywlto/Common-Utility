using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.sky88.web.automation
{
    public interface ITaskObserver
    {
        void notifyError(string message, Exception e);
        void notifyProgress(string message);
        void notifyComplete()
    }
}
