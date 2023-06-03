using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Dull
{
    public class ObjLoader
    {
        public List<Vector3> _verts = new List<Vector3>();
        public List<Vector3i> _faces = new List<Vector3i>();
        private List<Vector3> _texcoords = new List<Vector3>();
        private const string LINE_VERTEX_TYPE = "v";
        private const string LINE_FACE_TYPE = "f";
        private const string LINE_TEXTURE_TYPE = "vt";
        private static readonly char[] dataSeperators = new[] { ' ' };

        public bool Load(string path)
        {
            if (path == null) return false;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)) 
            {
                using (var file = new StreamReader(path))
                {
                    return ReadScene(file);
                }
            }
        }
        public bool ReadScene(StreamReader reader) 
        {
            int lineCounter = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lineCounter++;
                if (string.IsNullOrEmpty(line) || !TryReadLineType(line, out string type, out string data)) continue;

                if (type == LINE_VERTEX_TYPE)
                {
                    try
                    {
                        string[] dataStrings = data.Split(dataSeperators, StringSplitOptions.RemoveEmptyEntries);
                        _verts.Add(new Vector3
                            (
                                float.Parse(dataStrings[0], CultureInfo.InvariantCulture),
                                float.Parse(dataStrings[1], CultureInfo.InvariantCulture),
                                float.Parse(dataStrings[2], CultureInfo.InvariantCulture)
                            ));
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message + "at line " + lineCounter); return false; }
                }
                if (type == LINE_FACE_TYPE)
                {
                    try
                    {
                        string[] dataStrings = data.Split(dataSeperators, StringSplitOptions.RemoveEmptyEntries);
                        Vector4i vertex = new Vector4i();
                        for (int i = 0;i<dataStrings.Length;i++)
                        {
                            vertex[i] = int.Parse(dataStrings[i].Split(new[] { '/' }, StringSplitOptions.None)[0]);

                        }
                        _faces.Add(new Vector3i(vertex[0], vertex[1], vertex[2]));
                        //_faces.Add(new Vector3i(vertex[0], vertex[2], vertex[3]));
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message + "at line " + lineCounter); return false; }
                }

            }
            return true;
        }
        public static bool TryReadLineType(string line, out string type, out string data)
        {
            string stripedLine = line.TrimStart();
            for (int i = 0; i < stripedLine.Length; i++) 
            {
                if (char.IsWhiteSpace(stripedLine[i]))
                {
                    type = stripedLine.Substring(0, i);
                    data = stripedLine.Substring(i).Trim();
                    return true;
                }
            }
            type = null;
            data = null;
            return false;
        }  
    }
}
