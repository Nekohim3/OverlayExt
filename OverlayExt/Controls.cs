using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Overlay.Windows;
using OverlayExt.UI;

namespace OverlayExt
{
    public static class Controls
    {
        public static List<DxControl> ControlList = new List<DxControl>();

        public static void Add(DxControl ctrl)
        {
            ControlList.Add(ctrl);
        }

        public static void Remove(DxControl ctrl)
        {
            ControlList.Remove(ctrl);
        }

        public static void Draw()
        {
            foreach (var q in ControlList)
            {
                q.Draw();
            }
        }
    }
}
