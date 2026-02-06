using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homeLibrary.Helpers
{
    public static class TextHelpers
    {
        public static string NormalizeTitle(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            input = input.Trim().ToLower();

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words);
        }
        public static int GetLevenshteinDistance(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return string.IsNullOrEmpty(s2) ? 0 : s2.Length;
            if (string.IsNullOrEmpty(s2)) return s1.Length;

            int n = s1.Length, m = s2.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    int insertion = d[i, j - 1] + 1;
                    int deletion = d[i - 1, j] + 1;
                    int substitution = d[i - 1, j - 1] + cost;

                    d[i, j] = Math.Min(insertion, Math.Min(deletion, substitution));
                }
            }

            return d[n, m];
        }
    }
}
