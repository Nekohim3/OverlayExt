using System;
using System.Collections.Generic;
using System.Linq;
using Overlay.Drawing;

namespace OverlayExt
{
    public static class FontCollection
    {
        private static readonly List<FontItem> Fonts = new List<FontItem>();

        public static void Init()
        {
#if DEBUG
            g.Config.RemoveAll("Fonts");
            Fonts.Clear();
#endif
            try
            {
                foreach (var q in g.Config.GetKeys("Fonts"))
                    if(!q.EndsWith(".Size"))
                        Fonts.Add(new FontItem(q));
            }
            catch (Exception e)
            {
                g.Config.RemoveAll("Fonts");
                Fonts.Clear();
            }
        }

        public static FontItem Get(string name)
        {
            return Fonts.First(x => x.Name == name);
        }

        public static void Add(string name, string font, int size)
        {
            if (Fonts.Count(x => x.Name == name) == 0)
                Fonts.Add(new FontItem(name, font, size));
        }
    }

    public class FontItem
    {
        public NFont Font { get; set; }
        public string Name { get; set; }

        public FontItem(string name)
        {
            Name = name;
            Font = new NFont(g.Config.GetString("Fonts", Name), g.Config.GetInt("Fonts", $"{Name}.Size"));
        }

        public FontItem(string name, string font, int size)
        {
            Name = name;
            Font = new NFont(font, size);
            Save();
        }

        public void Set(string n, int s)
        {
            Font.Set(n, s);
            g.Config.Set("Fonts", Name, n);
            g.Config.Set("Fonts", $"{Name}.Size", s);
        }

        public void Set(string n)
        {
            Font.Set(n);
            g.Config.Set("Fonts", Name, n);
        }

        public void Set(int s)
        {
            Font.Set(s);
            g.Config.Set("Fonts", $"{Name}.Size", s);
        }

        public void Load() => Font.Set(g.Config.GetString("Fonts", Name), g.Config.GetInt("Fonts", $"{Name}.Size"));

        public void LoadDefault() => Font.Set(g.Config.GetString("Fonts", Name, true), g.Config.GetInt("Fonts", $"{Name}.Size", true));

        public void LoadDefaultSize() => Font.Set(g.Config.GetInt("Fonts", $"{Name}.Size", true));

        public void LoadDefaultFont() => Font.Set(g.Config.GetInt("Fonts", Name, true));

        public void Save()
        {
            g.Config.Set("Fonts", Name, Font.Name);
            g.Config.Set("Fonts", $"{Name}.Size", Font.Size);
        }
    }
}
