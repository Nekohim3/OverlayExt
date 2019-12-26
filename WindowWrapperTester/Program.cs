using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Overlay.Windows;
using OverlayExt;
using OverlayExt.UI;
using SharpDX.DirectWrite;
using SharpDX.IO;
using SharpDX.WIC;
using FontCollection = OverlayExt.FontCollection;
using HorizontalAlignment = OverlayExt.UI.HorizontalAlignment;

namespace WindowWrapperTester
{
    static class Program
    {
        public static bool Loaded;
        [STAThread]
        static void Main()
        {
            g.Overlay = new WindowWrapper(100, 100, 1280, 720)
            {
                UseHook = true,
                Window = { Title = "Test" },

            };
            g.Overlay.OnGraphicsSetup += Overlay_OnGraphicsSetup;
            g.Overlay.OnGraphicsDestroy += Overlay_OnGraphicsDestroy;

            g.Overlay.OnPreDraw += Overlay_OnPreDraw;
            g.Overlay.OnDraw += Overlay_OnDraw;

            g.Overlay.OnKeyDown += Overlay_OnKeyDown;
            g.Overlay.OnKeyUp += Overlay_OnKeyUp;

            g.Overlay.OnMouseDown += Overlay_OnMouseDown;
            g.Overlay.OnMouseUp += Overlay_OnMouseUp;
            g.Overlay.OnMouseMove += Overlay_OnMouseMove;
            g.Overlay.OnMouseWheel += Overlay_OnMouseWheel;
            g.Graphics = g.Overlay.Window.Graphics;
            g.Overlay.Run();

            Loaded = true;

            Application.Run();

        }

        #region overlay events

        private static SharpDX.Direct2D1.Bitmap bmp;
        private static int qq = 0;
        private static int ww = 0;
        private static DxLabel l;
        private static DxLabel l1;
        private static DxTextBox textbox;
        private static DxTextBox textbox1;
        private static DxTrackBar tb;
        private static void Overlay_OnGraphicsSetup(object sender, SetupGraphicsEventArgs e)
        {


            //textbox = new DxTextBox("2", "qwertyuiop[]asdfghjkl;'zxcvbnm,./1234567890") { Margin = new Thickness(650, 10, 0, 0), Width = 550 };
            //textbox1 = new DxTextBox("2", "qwertyuiop[]asdfghjkl;'zxcvbnm,./1234567890".ToUpperInvariant()) { Margin = new Thickness(650, 35, 0, 0), Width = 550 };
            //Controls.Add(textbox);
            //Controls.Add(textbox1);

            
            //l1 = new DxLabel("1", $"{g.Graphics.MeasureText(textbox.Text.Substring(0, textbox.CaretPos + 1), FontCollection.Get("Control.Font").Font).Width}") { Margin = new Thickness(10, 35, 0, 0), Width = 200 };
            //Controls.Add(l1);

            tb = new DxTrackBar("3", "DxTrackBar")
            {
                Margin = new Thickness(200,500,0,0),
                Min = 0,
                Max = 255,
                TickRate = 1,
                Value = 0,
                IsSnapToTick = true
            };
            Controls.Add(tb);
            //l = new DxLabel("1", $"")
            //{
            //    Margin = new Thickness(10, 10, 0, 0), Width = 800, Height = 500
            //};
            //Controls.Add(l);
        }

        private static void Overlay_OnGraphicsDestroy(object sender, DestroyGraphicsEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnPreDraw(object sender, DrawGraphicsEventArgs e)
        {
            if (!Loaded) return;
            //l.Text = $"{g.Graphics.MeasureText(textbox.Text.Substring(0, textbox.CaretPos), FontCollection.Get("Control.Font").Font).Width}";
            //l1.Text = $"{g.Graphics.MeasureText(textbox.Text.Substring(0, textbox.CaretPos + 1), FontCollection.Get("Control.Font").Font).Width}";
        }

        private static void Overlay_OnDraw(object sender, DrawGraphicsEventArgs e)
        {
            if (!Loaded) return;
        }

        private static void Overlay_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnMouseUp(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

        }

        private static void Overlay_OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (!Loaded) return;

        }

        #endregion
    }

    
}
