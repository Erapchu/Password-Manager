﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager.Model
{
    static class Encryption
    {
        public static int KEY { get; set; }

        public static string Process(string s)
        {
            byte[] buf = Encoding.Unicode.GetBytes(s);

            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(buf[i] ^ KEY);
            }
            string result = Encoding.Unicode.GetString(buf);
            return result;
        }

    }
}
