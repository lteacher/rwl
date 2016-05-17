using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RobotInitial.Components
{

    interface Component
    {
        Point Location { get; set; }
        Component Next { get; set; }
    }

    class AbstractComponent : Component
    {
        private Point location = new Point();
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        private Component next = null;

        public Component Next
        {
            get { return next; }
            set { next = value; }
        }

    }

    interface CompositeComponent : Component
    {
        List<Component> Children { get; }
        void addChild(Component child);

    }

    class AbstractCompositeComponent : AbstractComponent, CompositeComponent
    {
        private List<Component> children = new List<Component>();

        public List<Component> Children
        {
            get { return children; }
        }

        public void addChild(Component child)
        {
            children.Add(child);
        }

    }

    class StartComponent : AbstractComponent
    {

    }

    class MoveComponent : AbstractComponent
    {

    }

    class WaitComponent : AbstractComponent
    {

    }

    class LoopComponent : AbstractCompositeComponent
    {

    }

}
