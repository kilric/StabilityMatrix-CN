﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text;

namespace StabilityMatrix.Core.Extensions;

public static class StringExtensions
{
    private static string EncodeNonAsciiCharacters(string value) {
        var sb = new StringBuilder();
        foreach (var c in value) 
        {
            // If not ascii or not printable
            if (c > 127 || c < 32)
            {
                // This character is too big for ASCII
                var encodedValue = "\\u" + ((int) c).ToString("x4");
                sb.Append(encodedValue);
            }
            else {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
    
    /// <summary>
    /// Converts string to repr
    /// </summary>
    public static string ToRepr(this string? str)
    {
        if (str is null)
        {
            return "<null>";
        }
        using var writer = new StringWriter();
        writer.Write("'");
        foreach (var ch in str)
        {
            writer.Write(ch switch
            {
                '\0' => "\\0",
                '\n' => "\\n",
                '\r' => "\\r",
                '\t' => "\\t",
                // Non ascii
                _ when ch > 127 || ch < 32 => $"\\u{(int) ch:x4}",
                _ => ch.ToString()
            });
        }
        writer.Write("'");
        
        return writer.ToString();
    }
    
    /// <summary>
    /// Counts continuous sequence of a character
    /// from the start of the string
    /// </summary>
    public static int CountStart(this string str, char c)
    {
        var count = 0;
        foreach (var ch in str)
        {
            if (ch == c)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }
}