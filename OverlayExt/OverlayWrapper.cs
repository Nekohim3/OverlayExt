using System;
using System.Threading;
using System.Windows.Forms;
using Overlay.Drawing;
using Overlay.PInvoke;
using Overlay.Windows;
using NiniExt;

namespace OverlayExt
{
    public class OverlayWrapper : OverlayExt
    {


        #region events

        #region graphic

        public override event OverlayExt.GraphicsSetupHandler   OnGraphicsSetup;
        public override event OverlayExt.GraphicsDestroyHandler OnGraphicsDestroy;
        public override event OverlayExt.DrawHandler            OnDraw;
        public override event OverlayExt.DrawHandler            OnPreDraw;

        #endregion

        #region hook

        public override event OverlayExt.KeyHandler   OnKeyDown;
        public override event OverlayExt.KeyHandler   OnKeyUp;
        public override event OverlayExt.MouseHandler OnMouseDown;
        public override event OverlayExt.MouseHandler OnMouseUp;
        public override event OverlayExt.MouseHandler OnMouseMove;
        public override event OverlayExt.MouseHandler OnMouseWheel;

        #endregion

        #endregion

        private bool Graphicloaded;
        private bool WindowLoaded;


        public OverlayWrapper(int x, int y, int width, int height) : base(x, y, width, height)
        {
            LoadConfig();

            var graphics = new Graphics
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = false,
                TextAntiAliasing = true,
                UseMultiThreadedFactories = false,
                VSync = false,
                WindowHandle = IntPtr.Zero
            };

            Window = new GraphicsWindow(graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = g.Config.GetInt("System", "fps"),
                X = x,
                Y = y,
                Width = width,
                Height = height,
                EX_WS = ExtendedWindowStyle.Transparent | ExtendedWindowStyle.Layered | ExtendedWindowStyle.ToolWindow
            };


            Window.SetupGraphics += _window_SetupGraphics;
            Window.DestroyGraphics += _window_DestroyGraphics;
            Window.DrawGraphics += _window_DrawGraphics;
        }

        public void LoadConfig()
        {
            g.Config = new IniConfig("Config.ini");
            g.Config.AddConfig("Brushes", true);
            g.Config.AddConfig("Fonts", true);
            g.Config.AddConfig("System", true);
            g.Config.SetNew("System", "fps", 60, 60);
        }
        

        public override void Run()
        {
            base.Run();
            while (!Graphicloaded)
            {
                Thread.Sleep(1);
            }

            WindowLoaded = true;
        }

        
        internal override void GHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Loaded) return;

            OnKeyDown?.Invoke(sender, e);
        }

        internal override void GHook_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Loaded) return;

            OnKeyUp?.Invoke(sender, e);
        }

        internal override void GHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

            OnMouseDown?.Invoke(sender, e);
        }

        internal override void GHook_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

            OnMouseUp?.Invoke(sender, e);
        }

        internal override void GHook_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

            OnMouseMove?.Invoke(sender, e);
        }

        internal override void GHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

            OnMouseWheel?.Invoke(sender, e);
        }

        internal override void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e)
        {
            while (!Loaded)
            {
                Thread.Sleep(1);
            }

            BrushCollection.Init();
            FontCollection.Init();

            OnGraphicsSetup?.Invoke(sender, e);

            Graphicloaded = true;
        }

        internal override void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            if (!Loaded) return;
            if (!WindowLoaded) return;

            OnPreDraw?.Invoke(sender, e);
            
            e.Graphics.ClearScene();

            OnDraw?.Invoke(sender, e);

        }

        internal override void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
        {
            if (!Loaded) return;

            OnGraphicsDestroy?.Invoke(sender, e);
        }

        ~OverlayWrapper()
        {
            // you do not need to dispose the Graphics surface
            Window.Dispose();
        }
    }
}
