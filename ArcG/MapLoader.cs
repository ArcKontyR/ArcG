using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcG
{
    public static class MapLoader
    {
        public static Dictionary<Vector2, int> LoadMap(string filePath)
        {
            Dictionary<Vector2, int> map = new();
            StreamReader reader = new(filePath);

            int y = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] lines = line.Split(",");
                for (int x = 0; x < lines.Length; x++)
                {
                    Trace.WriteLine($"({x},{y})");
                    if (int.TryParse(lines[x], out int value))
                    {
                        if (value < 0) continue;
                        map[new Vector2(x, y)] = value;
                    }
                }
                y++;
            }
            return map;
        }
    }
}
