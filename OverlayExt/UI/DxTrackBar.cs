using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Overlay.Drawing;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Point = Overlay.Drawing.Point;
using Rectangle = Overlay.Drawing.Rectangle;

namespace OverlayExt.UI
{
    public class DxTrackBar : DxControl
    {

        #region Events

        public delegate void TrackBarEventHandler(DxTrackBar tb);

        public event TrackBarEventHandler ValueChanged;

        public virtual void OnValueChanged(DxTrackBar tb)
        {
            SliderPos.X = (int)(Rect.X + 3 + (Value - Min) * (Max - Min) / (Width - 10) / TickRate);
            SliderPos.Y = Rect.Y + 2;
            SliderPos.Width = 5;
            SliderPos.Height = Rect.Height - 4;
            ValueChanged?.Invoke(tb);
        }

        #endregion

        public SolidBrush FontBrush { get; set; }
        public NFont      Font      { get; set; }

        public SolidBrush FillBrush              { get; set; }
        public SolidBrush StrokeBrush            { get; set; }
        public SolidBrush FillHoverBrush         { get; set; }
        public SolidBrush StrokeHoverBrush       { get; set; }
        public SolidBrush BarFillBrush           { get; set; }
        public SolidBrush BarFillHoverBrush      { get; set; }
        public SolidBrush BarStrokeBrush         { get; set; }
        public SolidBrush BarStrokeHoverBrush    { get; set; }
        public SolidBrush SliderFillBrush        { get; set; }
        public SolidBrush SliderFillHoverBrush   { get; set; }
        public SolidBrush SliderStrokeBrush      { get; set; }
        public SolidBrush SliderStrokeHoverBrush { get; set; }


        private float _tickRate;
        private float _max;
        private float _min;
        private float _value;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value < _min)
                    _value = _min;
                if (_value > _max)
                    _value = _max;
            }
        }
        public float Min
        {
            get => _min;
            set
            {
                _min = value;
                if (_min >= _max)
                    _max = _min + TickRate;
            }
        }
        public float Max
        {
            get => _max;
            set
            {
                _max = value;
                if (_max <= _min)
                    _min = _max - TickRate;
            }
        }
        public float TickRate
        {
            get => _tickRate;
            set
            {
                _tickRate = value;
                if (_tickRate > _max - _min)
                    _tickRate = _max - _min;
            }
        }

        public ValueType Type { get; set; }

        public ControlRectangle SliderPos { get; set; }

        public string Text    { get; set; }

        public bool IsMouseOverSlider { get; set; }

        public DxTrackBar(string name, string text) : base(name)
        {
            Width     = 200;
            Height    = 22;
            Margin    = new Thickness(11, 11, 1, 1);
            Text      = text;
            SliderPos = new ControlRectangle(0, 0, 0, 0);
            _min       = 0;
            _max       = 10;
            _tickRate  = 1;
            _value     = 0;

            Font      = FontCollection.Get("TrackBar.Font").Font;
            FontBrush = BrushCollection.Get("TrackBar.Font").Brush;

            FillBrush              = BrushCollection.Get("TrackBar.Fill").Brush;
            StrokeBrush            = BrushCollection.Get("TrackBar.Stroke").Brush;
            FillHoverBrush         = BrushCollection.Get("Control.Fill.MouseOver").Brush;
            StrokeHoverBrush       = BrushCollection.Get("Control.Stroke.MouseOver").Brush;
            BarFillBrush           = BrushCollection.Get("TrackBar.Bar.Fill").Brush;
            BarFillHoverBrush      = BrushCollection.Get("TrackBar.Bar.Fill.Hover").Brush;
            BarStrokeBrush         = BrushCollection.Get("TrackBar.Bar.Stroke").Brush;
            BarStrokeHoverBrush    = BrushCollection.Get("TrackBar.Bar.Stroke.Hover").Brush;
            SliderFillBrush        = BrushCollection.Get("TrackBar.Slider.Fill").Brush;
            SliderFillHoverBrush   = BrushCollection.Get("TrackBar.Slider.Fill.Hover").Brush;
            SliderStrokeBrush      = BrushCollection.Get("TrackBar.Slider.Stroke").Brush;
            SliderStrokeHoverBrush = BrushCollection.Get("TrackBar.Slider.Stroke.Hover").Brush;

        }

        public override void Draw()
        {
            if (IsMouseOver)
            {
                g.Graphics.OutlineFillRectangle(StrokeHoverBrush,    FillHoverBrush,    Rect.X,     Rect.Y,     Rect.Width,     Rect.Height,     1, 0);
                g.Graphics.OutlineFillRectangle(BarStrokeHoverBrush, BarFillHoverBrush, Rect.X + 3, Rect.Y + 3, Rect.Width - 6, Rect.Height - 6, 2, 0);
            }
            else
            {
                g.Graphics.OutlineFillRectangle(StrokeBrush,    FillBrush,    Rect.X,     Rect.Y,     Rect.Width,     Rect.Height,     1, 0);
                g.Graphics.OutlineFillRectangle(BarStrokeBrush, BarFillBrush, Rect.X + 3, Rect.Y + 3, Rect.Width - 6, Rect.Height - 6, 2, 0);
            }

            if (IsMouseOverSlider)
                g.Graphics.OutlineFillRectangle(SliderStrokeHoverBrush, SliderFillHoverBrush, SliderPos.X, SliderPos.Y, SliderPos.Width, SliderPos.Height, 1, 0);
            else
                g.Graphics.OutlineFillRectangle(SliderStrokeBrush, SliderFillBrush, SliderPos.X, SliderPos.Y, SliderPos.Width, SliderPos.Height, 1, 0);



            g.Graphics.DrawText(Text,                            Rect.X + 7, Rect.Y + 5, Rect.Width - 14, Rect.Height - 10, Font, FontBrush, ta: TextAlignment.Leading,  pa: ParagraphAlignment.Center);
            g.Graphics.DrawText(Math.Round(Value, 3).ToString(), Rect.X + 7, Rect.Y + 5, Rect.Width - 14, Rect.Height - 10, Font, FontBrush, ta: TextAlignment.Trailing, pa: ParagraphAlignment.Center);
        }

        public override bool IntersectTest(int x, int y)
        {
            var res = base.IntersectTest(x, y);
            IsMouseOverSlider = res && x >= SliderPos.X && x <= SliderPos.X + SliderPos.Width && y >= SliderPos.Y && y <= SliderPos.Y + SliderPos.Height;
            return res;
        }

        public override void OnMouseDown(DxControl ctl, MouseEventArgs e, Point pt)
        {
            base.OnMouseDown(ctl, e, pt);
            if (IsMouseDown && IsMouseOver)
                CalcNewValue(pt.X);
        }

        public override void OnMouseMove(DxControl ctl, MouseEventArgs e, Point pt)
        {
            if (IsMouseDown && IsMouseOver)
                CalcNewValue(pt.X);
        }

        private void CalcNewValue(float x)
        {
            var mouseBarPosition = x - (Rect.X + SliderPos.Width / 2       + 3);
            var newValue         = mouseBarPosition * (Max - Min) / (Width - 10);
            var q = newValue / (Max - Min);
            Value = newValue + Min;

            if (newValue >= Min && newValue <= Max && Math.Abs(newValue - Value) > 0.00001)
            {
                Value = newValue + Min;
                OnValueChanged(this);
            }
        }

        private int GetFloatPrecision(float f)
        {
            return f.ToString().Split(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0])[3].Length;
        }
    }
}
