using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace QLBH_Dion.Util
{
    public static class StringExtension
    {
        public static Random random = new Random();
        public static string RemoveVietnamese(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static string ConvertToSlug(this string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text.Replace(' ', '-');
        }

        public static string ToHash256(this string rawText)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawText));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string RandomString(int length)
        {
            var randomString = string.Empty;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            randomString = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }

        public static string ToEscape(this string htmlContent)
        {
            StringBuilder sb = new StringBuilder(htmlContent);
            sb.Replace("<script>", "&lt;script&gt;");
            sb.Replace("</script>", "&lt;/script&gt;");
            return sb.ToString();
        }
        /// <summary>
        /// Author: TUNGTD
        /// Created: 26/07/2023
        /// Description: Regex max match expression
        /// </summary>
        /// <param name="expression">Regex expression</param>
        /// <returns></returns>
        public static bool IsMatch(this string text, string expression)
        {
            Regex re = new Regex(expression);
            return re.IsMatch(text);
        }
        /// <summary>
        /// Author: TUNGTD
        /// Created: 30/07/2023
        /// Description: Validate path string name
        /// </summary>
        /// <param name="pathName">path name</param>
        /// <returns>true when path is valid</returns>
        /// <returns>false when path is not valid</returns>
        [Obsolete]
        public static bool IsValidFilename(this string pathName)
        {
            Regex containsABadCharacter = new Regex($"[{Regex.Escape(string.Join("", System.IO.Path.InvalidPathChars))}]");
            if (containsABadCharacter.IsMatch(pathName)) { return false; };
            return true;
        }
        /// <summary>
        /// Get Valid folder Name
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public static string GetValidFolderName(this string pathName)
        {
            pathName = pathName.Trim().RemoveVietnamese().Replace(" ", "_");
            foreach (var key in System.IO.Path.InvalidPathChars)
            {
                pathName = pathName.Replace(key, '_');
            }
            if (string.IsNullOrEmpty(pathName))
            {
                throw new Exception("folder path is not valid or empty.");
            }
            return pathName;
        }
        /// <summary>
        /// Author: TUNGTD
        /// Created: 31/07/2023
        /// Description: convert web path to folder path
        /// </summary>
        /// <param name="path">web format path</param>
        /// <returns></returns>
        public static string GetFolderFormatPath(this string path)
        {
            return path.Replace("/", @"\");
        }
        /// <summary>
        /// Author: TUNGTD
        /// Created: 31/07/2023
        /// Description: convert folder path to web path
        /// </summary>
        /// <param name="path">folder path</param>
        /// <returns></returns>
        public static string GetWebFormatPath(this string path)
        {
            return path.Replace(@"\", "/");
        }


    }
}
