﻿using System.Windows.Forms;
using Overlay.Drawing;
using SharpDX.DirectWrite;

namespace OverlayExt.UI
{
    public class DxButton : DxControl
    {
        #region Events

        public delegate void ButtonEventHandler(DxButton btn);

        public event ButtonEventHandler Click;

        public virtual void OnClick(DxButton btn)
        {
            Click?.Invoke(btn);
        }

        #endregion

        #region Brushes

        public NFont Font { get; set; }
        public SolidBrush FontBrush { get; set; }
        public SolidBrush FillBrush { get; set; }
        public SolidBrush StrokeBrush { get; set; }
        public SolidBrush HoverFillBrush { get; set; }
        public SolidBrush HoverStrokeBrush { get; set; }
        public SolidBrush PressedFillBrush { get; set; }
        public SolidBrush PressedStrokeBrush { get; set; }

        #endregion

        #region Public Properties

        public string Text { get; set; }
        public TextAlignment TextAlign { get; set; }
        public ParagraphAlignment ParagraphAlign { get; set; }

        #endregion

        public DxButton(string name, string text) : base(name)
        {
            Width = 100;
            Height = 22;
            Margin = new Thickness(11, 11, 1, 1);

            Text = text;

            TextAlign = TextAlignment.Center;
            ParagraphAlign = ParagraphAlignment.Center;

            Font = FontCollection.Get("Control.Font").Font;
            FontBrush = BrushCollection.Get("Control.Font").Brush;

            FillBrush = BrushCollection.Get("Control.Fill").Brush;
            StrokeBrush = BrushCollection.Get("Control.Stroke").Brush;
            HoverFillBrush = BrushCollection.Get("Control.Fill.MouseOver").Brush;
            HoverStrokeBrush = BrushCollection.Get("Control.Stroke.MouseOver").Brush;
            PressedFillBrush = BrushCollection.Get("Control.Fill.Pressed").Brush;
            PressedStrokeBrush = BrushCollection.Get("Control.Stroke.Pressed").Brush;

        }

        public override void Draw()
        {
            if (IsMouseOver)
            {
                if (IsMouseDown)
                    g.Graphics.OutlineFillRectangle(PressedStrokeBrush, PressedFillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);
                else
                    g.Graphics.OutlineFillRectangle(HoverStrokeBrush, HoverFillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);
            }
            else
                g.Graphics.OutlineFillRectangle(StrokeBrush, FillBrush, Rect.X, Rect.Y, Rect.Width, Rect.Height, 1, 0);
            
            g.Graphics.DrawText(Text, Rect.X, Rect.Y, Rect.Width, Rect.Height, Font, FontBrush, TextAlign, ParagraphAlign);
        }

        public override void OnMouseDown(DxControl ctl, MouseEventArgs args, Point pt)
        {
            //TODO: make normal click (down + up) and double click?
            OnClick(this);
            base.OnMouseDown(ctl, args, pt);
        }
    }
}
