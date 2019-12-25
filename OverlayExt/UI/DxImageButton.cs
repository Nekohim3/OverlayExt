using System.Windows.Forms;
using Overlay.Drawing;
using SharpDX.Direct2D1;

namespace OverlayExt.UI
{
    public class DxImageButton : DxControl
    {
        //#region Events

        //public delegate void ButtonEventHandler(DxButton btn);

        //public event ButtonEventHandler Click;

        //public virtual void OnClick(DxButton btn)
        //{
        //    Click?.Invoke(btn);
        //}

        //#endregion


        public SolidBrush FillBrush { get; set; }
        public SolidBrush StrokeBrush { get; set; }
        public SolidBrush MouseOverFillBrush { get; set; }
        public SolidBrush MouseOverStrokeBrush { get; set; }
        public SolidBrush PressedFillBrush { get; set; }
        public SolidBrush PressedStrokeBrush { get; set; }

        public Bitmap Image { get; set; }


        public DxImageButton(string name, Bitmap image) : base(name)
        {
            Width = 100;
            Height = 22;
            Margin = new Thickness(11, 11, 1, 1);

            Image = image;

            FillBrush = BrushCollection.Get("Control.Fill").Brush;
            StrokeBrush = BrushCollection.Get("Control.Stroke").Brush;
            MouseOverFillBrush = BrushCollection.Get("Control.Fill.MouseOver").Brush;
            MouseOverStrokeBrush = BrushCollection.Get("Control.Stroke.MouseOver").Brush;
            PressedFillBrush = BrushCollection.Get("Control.Fill.Pressed").Brush;
            PressedStrokeBrush = BrushCollection.Get("Control.Stroke.Pressed").Brush;

        }

        public override void Draw()
        {
            //base.Draw();
            if (IsMouseOver)
            {
                if (IsMouseDown)
                    g.Graphics.OutlineFillRectangle(PressedStrokeBrush, PressedFillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);
                else
                    g.Graphics.OutlineFillRectangle(MouseOverStrokeBrush, MouseOverFillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);
            }
            else
                g.Graphics.OutlineFillRectangle(StrokeBrush, FillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);

            var scale = 1f;
            if(Rect.Width < Image.Size.Width)
                scale = Rect.Width / Image.Size.Width;
            if (Rect.Height < Image.Size.Height * scale)
                scale = Rect.Height / Image.Size.Height;

            if(Image != null)
                g.Graphics.DrawImage(Image, Rect.X + (Rect.Width / 2) - (Image.Size.Width * scale / 2), Rect.Y + (Rect.Height / 2) - (Image.Size.Height * scale / 2), scale);
        }

        public override void OnMouseDown(DxControl ctl, MouseEventArgs args, Point pt)
        {
            //MouseDown?.Invoke(ctl, args);
            base.OnMouseDown(ctl, args, pt);
        }
    }
}
