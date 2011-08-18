using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotInitial.Events
{
    interface EventPublisher<T> where T : EventArgs
    {
        event EventHandler<T> RaiseEvent;
    }

    interface EventSubscriber<T> where T : EventArgs
    {
        void subscribe(EventPublisher<T> pub);
        void handle(Object src, T t);
    }

    abstract class ComponentEvent : EventArgs
    {
    }

 

    class MoveEvent : ComponentEvent
    {
    }

    class CopyEvent : ComponentEvent
    {
    }

    class CreateEvent : ComponentEvent
    {
    }

    class DeleteEvent : ComponentEvent
    {
    }

    abstract class ProjectEvent : EventArgs
    {
    }

    class Save { }
}