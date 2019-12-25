using System.Windows.Forms;
using Overlay.Drawing;
using SharpDX.DirectWrite;

namespace OverlayExt.UI
{
    public class DxLabel : DxControl
    {
        public SolidBrush FontBrush { get; set; }
        public NFont Font { get; set; }


        public SolidBrush FillBrush { get; set; }
        public SolidBrush StrokeBrush { get; set; }
        public SolidBrush MouseOverFillBrush { get; set; }
        public SolidBrush MouseOverStrokeBrush { get; set; }
        public SolidBrush PressedFillBrush { get; set; }
        public SolidBrush PressedStrokeBrush { get; set; }

        public string Text { get; set; }

        public TextAlignment TextAlign { get; set; }
        public ParagraphAlignment ParagraphAlign { get; set; }

        public DxLabel(string name, string text) : base(name)
        {
            Width = 100;
            Height = 22;
            Margin = new Thickness(11, 11, 1, 1);

            Text = text;

            TextAlign = TextAlignment.Center;
            ParagraphAlign = ParagraphAlignment.Center;

            Font = FontCollection.Get("Control.Font").Font;
            FontBrush = BrushCollection.Get("Control.Font").Brush;

            FillBrush = BrushCollection.Get("Control.Transparent").Brush;
            StrokeBrush = BrushCollection.Get("Control.Transparent").Brush;
            MouseOverFillBrush = BrushCollection.Get("Control.Transparent").Brush;
            MouseOverStrokeBrush = BrushCollection.Get("Control.Transparent").Brush;
            PressedFillBrush = BrushCollection.Get("Control.Transparent").Brush;
            PressedStrokeBrush = BrushCollection.Get("Control.Transparent").Brush;
            IsTransparent = true;

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

            g.Graphics.DrawText(Text, Rect.X, Rect.Y, Rect.Width, Rect.Height, Font, FontBrush, TextAlign, ParagraphAlign);
        }
    }
}
