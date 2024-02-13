using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace BackgroundTaskHelper.Models
{
    public class CommonHelper
    {
        public enum EnRoles
        {
            SuperAdmin = 1,
            CounsellingAdmin = 2,
            Applicant = 3,
            Report = 4,

        }


        public static string Set_Paging(int PageNumber, int PageSize, long TotalRecords, string ClassName, string PageUrl, string DisableClassName)
        {
            StringBuilder ReturnValue = new StringBuilder();

            long TotalPages = Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
            if (PageNumber > 1)
            {
                if (PageNumber == 2)
                {
                    ReturnValue.Append("<a href='").Append(PageUrl.Trim()).Append("?pn=").Append(Convert.ToString(PageNumber - 1)).Append("' class='").Append(ClassName).Append("' >Previous</a>   ");
                }
                else
                {
                    ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                    if (PageUrl.Contains("?"))
                        ReturnValue.Append("&");
                    else
                        ReturnValue.Append("?");
                    ReturnValue.Append("pn=").Append(Convert.ToString(PageNumber - 1)).Append("' class='").Append(ClassName).Append("' >Previous</a>   ");
                }
            }
            else
            {
                ReturnValue.Append("<span class='").Append(DisableClassName).Append("'>Previous</span>   ");
            }

            if ((PageNumber - 3) > 1)
            {
                ReturnValue.Append("<a href='").Append(PageUrl.Trim()).Append("' class='").Append(ClassName).Append("' >1</a> ..... | ");
            }

            for (int i = PageNumber - 3; i <= PageNumber; i++)
            {
                if (i >= 1)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                        if (PageUrl.Contains("?"))
                            ReturnValue.Append("&");
                        else
                            ReturnValue.Append("?");
                        ReturnValue.Append("pn=").Append(i.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(i.ToString()).Append("</a> | ");
                    }
                    else
                    {
                        ReturnValue.Append("<span style='font-weight:bold;'>").Append(i).Append("</span> | ");
                    }
                }
            }

            for (int i = PageNumber + 1; i <= PageNumber + 3; i++)
            {
                if (i <= TotalPages)
                {
                    if (PageNumber != i)
                    {
                        ReturnValue.Append("<a href='").Append(PageUrl.Trim());
                        if (PageUrl.Contains("?"))
                            ReturnValue.Append("&");
                        else
                            ReturnValue.Append("?");
                        ReturnValue.Append("pn=").Append(i.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(i.ToString()).Append("</a> | ");
                    }
                    else
                    {
                        ReturnValue.Append("<span style='font-weight:bold;'>").Append(i).Append("</span> | ");
                    }
                }
            }

            if ((PageNumber + 3) < TotalPages)
            {
                ReturnValue.Append("..... <a href='").Append(PageUrl.Trim());
                if (PageUrl.Contains("?"))
                    ReturnValue.Append("&");
                else
                    ReturnValue.Append("?");
                ReturnValue.Append("pn=").Append(TotalPages.ToString()).Append("' class='").Append(ClassName).Append("' >").Append(TotalPages.ToString()).Append("</a>");
            }
            if (PageNumber < TotalPages)
            {
                ReturnValue.Append("   <a href='").Append(PageUrl.Trim());
                if (PageUrl.Contains("?"))
                    ReturnValue.Append("&");
                else
                    ReturnValue.Append("?");
                ReturnValue.Append("pn=").Append(Convert.ToString(PageNumber + 1)).Append("' class='").Append(ClassName).Append("' >Next</a>");
            }
            else
            {
                ReturnValue.Append("   <span class='").Append(DisableClassName).Append("'>Next</span>");
            }

            return ReturnValue.ToString();
        }



        /// <summary>
        /// Function to check if the file mime type is same as per the extensions allowed in the page
        /// </summary>
        /// <param name="file">The file object that is uploaded by user</param>
        /// <param name="arrAllowedExtension">types of extensions allowed in the page</param>
        /// <returns>Boolean as in true and false</returns>
        public static bool IsFileValid(IFormFile file, string[] arrAllowedExtension)
        {
            string[] arrAllowedMime = new string[arrAllowedExtension.Length];
            for (int cnt = 0; cnt < arrAllowedExtension.Length; cnt++)
            {
                arrAllowedMime[cnt] = GetMimeTypeByFileExtension(arrAllowedExtension[cnt]);
            }
            StringCollection imageTypes = new StringCollection();
            StringCollection imageExtension = new StringCollection();
            imageTypes.AddRange(arrAllowedMime);
            imageExtension.AddRange(arrAllowedExtension);
            string filename = file.FileName;

            //to calculate dots
            int count = filename.Count(f => f == '.');
            string strFiletype = file.ContentType;

            string fileExt = Path.GetExtension(Path.GetExtension(file.FileName).ToLower());

            return imageTypes.Contains(strFiletype) && imageExtension.Contains(fileExt) && count == 1;
        }

        /// <summary>
        /// Check if the file name is containing any dots
        /// </summary>
        ///<param name="strFilename">the actual name of file provided by the user</param>
        /// <returns>boolean value</returns>
        public static bool IsFilNameHavingDots(string strFilename)
        {
            //to calculate dots
            int count = strFilename.Count(f => f == '.');
            return count <= 0;
        }

        /// <summary>
        /// Function to get the mime type of file by extension
        /// </summary>
        /// <param name="strExtension">File extension with .</param>
        /// <returns>mime type with string format</returns>
        public static string GetMimeTypeByFileExtension(string strExtension)
        {
            string strMimeType = string.Empty;
            switch (strExtension.ToUpper())
            {
                case ".PDF":
                    strMimeType = "application/pdf";
                    break;
                case ".PNG":
                    strMimeType = "image/png";
                    break;
                case ".JPG":
                case ".JPEG":
                    strMimeType = "image/jpeg";
                    break;
                case ".ZIP":
                    strMimeType = "application/x-zip-compressed";
                    break;
                case ".RAR":
                    strMimeType = "application/x-rar-compressed";
                    break;
                case ".DOC":
                    strMimeType = "application/msword";
                    break;
                case ".DOCX":
                    strMimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".XLSX":
                    strMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".XLS":
                    strMimeType = "application/vnd.ms-excel";
                    break;
                case ".TXT":
                    strMimeType = "text/plain";
                    break;
                case ".GIF":
                    strMimeType = "image/gif";
                    break;
                case ".CSV":
                    strMimeType = "text/csv";
                    break;
                case ".LOG":
                    strMimeType = "text/plain";
                    break;
                case ".TIF":
                case ".TIFF":
                    strMimeType = "image/tiff";
                    break;
                case ".JSON":
                    strMimeType = "text/plain";
                    break;
            }
            return strMimeType;
        }

        public static void LogError(Exception ex, string strModule, string path)
        {
            string strFileName = "ErrorLog" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite!.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            path += "/ErrorLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }

        public static void LogOOAPIResponse(string content, string path)
        {
            string strFileName = "OOAPI" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;

            message += string.Format("Message: {0}", content);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            path += "/ErrorLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }

        public static void LogQuery(string Query, string path)
        {
            string strFileName = "QueryLog" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += Query;
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            path += "/QueryLog/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName;
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }
        protected static String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512")!;
            byte[] pp = sha1.ComputeHash(encPwd);
            // static string result =
            System.Text.Encoding.UTF8.GetString(pp);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        protected static String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512")!;
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
    }
}
