using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Diagnostics;

using RobotInitial.Components;

namespace RobotInitial
{
    class _Workspace : FrameworkElement
    {
        Project project = new Project();

        DragEventHandler handler;

        public _Workspace()
        {
            handler = new DragEventHandler(this.OnDrop);
            Drop += handler;
			this.Drop +=new DragEventHandler(this.OnDrop);
			Debug.WriteLine("Something Else");
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Debug.WriteLine("Entering ProjectRender.renderProject() ...");
            ProjectRenderer.renderProject(project, drawingContext);
            Debug.WriteLine("Exiting ProjectRender.renderProject() ...");
            //drawingContext.DrawText(new FormattedText("Woohoo Hi (Robot) World",System.Globalization.CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Monospaced"), 50, Brushes.Black), new Point(0, 0));
        }

        void OnDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("Something");
        }

    }

    class ProjectRenderer
    {
        const int TILE = 25;
        static Rect rect = new Rect(new Size(3*TILE, 3*TILE));

        public static void renderProject(Project project, DrawingContext context)
        {
            Debug.WriteLine("Project has " + project.StartComponents.Count + " start components");
            foreach (Component c in project.StartComponents)
            {
                Component _c = c;
                while (_c != null)
                {
                    // As Dynamic allows us to defer overloaded method resolution
                    // until runtime, instead of compile time. No more Visitor Pattern!
                    renderComponent(_c as dynamic, context);
                    _c = _c.Next;
                }
            }

        }

        static void renderComponent(StartComponent component, DrawingContext context)
        {
            
            rect.Location = new Point(((Component)component).Location.X*TILE, ((Component)component).Location.Y*TILE);
            Debug.WriteLine(rect.Location);
            context.DrawRoundedRectangle(Brushes.Red, new Pen(Brushes.Black, 1), rect, 4, 4);
        }

        static void renderComponent(MoveComponent component, DrawingContext context)
        {
            rect.Location = new Point(((Component)component).Location.X * TILE, ((Component)component).Location.Y * TILE);
            Debug.WriteLine(rect.Location);
            context.DrawRoundedRectangle(Brushes.Yellow, new Pen(Brushes.Black, 1), rect, 4, 4);
        }
        
        static void renderComponent(WaitComponent component, DrawingContext context)
        {
            rect.Location = new Point(((Component)component).Location.X * TILE, ((Component)component).Location.Y * TILE);
            Debug.WriteLine(rect.Location);
            context.DrawRoundedRectangle(Brushes.Blue, new Pen(Brushes.Black, 1), rect, 4, 4);
        }
        
        static void renderComponent(LoopComponent component, DrawingContext context)
        {
            rect.Location = new Point(((Component)component).Location.X * TILE, ((Component)component).Location.Y * TILE);
            Debug.WriteLine(rect.Location);
            context.DrawRoundedRectangle(Brushes.Green, new Pen(Brushes.Black, 1), rect, 4, 4);
        }

    }

    class Project
    {

        List<Component> startList = new List<Component>();

        public Project()
        {
            Component startComponent = new StartComponent();
			//startComponent.Location = new Point(3, 3);
			//startList.Add(startComponent);
			//startComponent.Next = new MoveComponent();
			//startComponent.Next.Location = new Point(7, 3);
			//startComponent.Next.Next = new WaitComponent();
			//startComponent.Next.Next.Location = new Point(11, 3);
			//startComponent.Next.Next.Next = new LoopComponent();
			//startComponent.Next.Next.Next.Location = new Point(15, 3);

			//startComponent = new MoveComponent();
			//startList.Add(startComponent);
			//startComponent.Location = new Point(10, 9);
        }

        public List<Component> StartComponents
        { 
            get { return startList; } 
        }
       
    }

}
