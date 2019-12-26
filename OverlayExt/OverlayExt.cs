using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Overlay.Windows;
using Gma.System.MouseKeyHook;

namespace OverlayExt
{
    public abstract class OverlayExt
    {
        #region events

        #region graphic

        public delegate void GraphicsSetupHandler(object sender, SetupGraphicsEventArgs e);

        public delegate void GraphicsDestroyHandler(object sender, DestroyGraphicsEventArgs e);

        public delegate void DrawHandler(object sender, DrawGraphicsEventArgs e);

        public virtual event GraphicsSetupHandler   OnGraphicsSetup;
        public virtual event GraphicsDestroyHandler OnGraphicsDestroy;
        public virtual event DrawHandler            OnDraw;
        public virtual event DrawHandler            OnPreDraw;

        #endregion

        #region hook

        public delegate void KeyHandler(object sender, KeyEventArgs e);

        public virtual event KeyHandler OnKeyDown;
        public virtual event KeyHandler OnKeyUp;

        public delegate void MouseHandler(object sender, MouseEventArgs e);

        public virtual event MouseHandler OnMouseDown;
        public virtual event MouseHandler OnMouseUp;
        public virtual event MouseHandler OnMouseMove;
        public virtual event MouseHandler OnMouseWheel;

        #endregion

        #endregion

        public GraphicsWindow Window;


        public IKeyboardMouseEvents GHook;
        public bool                 UseHook;

        internal bool Loaded;

        protected OverlayExt()
        {

        }

        protected OverlayExt(int x, int y, int width, int height)
        {

        }

        public virtual void Run()
        {
            g.Overlay  = this;
            g.Graphics = Window.Graphics;
            g.Window   = Window;
            Window.StartThread();
            while (!Window.Graphics.IsInitialized)
                Thread.Sleep(10);
            if (UseHook)
                StartHook();
            Loaded = true;
        }

        public virtual void StartHook()
        {
            GHook            =  Hook.GlobalEvents();
            GHook.MouseDown  += GHook_MouseDown;
            GHook.MouseUp    += GHook_MouseUp;
            GHook.KeyDown    += GHook_KeyDown;
            GHook.KeyUp      += GHook_KeyUp;
            GHook.MouseMove  += GHook_MouseMove;
            GHook.MouseWheel += GHook_MouseWheel;
        }

        internal abstract void GHook_KeyDown(object sender, KeyEventArgs e);

        internal abstract void GHook_KeyUp(object sender, KeyEventArgs e);

        internal abstract void GHook_MouseDown(object sender, MouseEventArgs e);

        internal abstract void GHook_MouseUp(object sender, MouseEventArgs e);

        internal abstract void GHook_MouseMove(object sender, MouseEventArgs e);

        internal abstract void GHook_MouseWheel(object sender, MouseEventArgs e);

        internal abstract void _window_SetupGraphics(object sender, SetupGraphicsEventArgs e);

        internal abstract void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e);

        internal abstract void _window_DestroyGraphics(object sender, DestroyGraphicsEventArgs e);

        ~OverlayExt()
        {
            Window.Dispose();
        }
    }

    
}
