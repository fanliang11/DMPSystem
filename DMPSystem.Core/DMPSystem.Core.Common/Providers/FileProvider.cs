using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPSystem.Core.Common.Providers
{
   public class FileProvider
    {
       public static void AppendText(string filePath, string text)
       {
           var writer = TextWriter.Synchronized(new StreamWriter(filePath, true));
           writer.WriteLine(text);
           writer.Flush();
       }

        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                string[] directories = GetDirectories(directoryPath);
                for (int j = 0; j < directories.Length; j++)
                {
                    DeleteDirectory(directories[j]);
                }
            }
        }

        public static void ClearFile(string filePath)
        {
            File.Delete(filePath);
            CreateFile(filePath);
        }

        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                if (GetFileNames(directoryPath, searchPattern, false).Length == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (GetFileNames(directoryPath, searchPattern, true).Length == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void CopyTo(string sourceFilePath, string destFilePath)
        {
            if (IsExistFile(sourceFilePath))
            {
                try
                {
                    CreateDirectory(GetDirectoryFromFilePath(destFilePath));
                    new FileInfo(sourceFilePath).CopyTo(destFilePath, true);
                }
                catch
                {
                }
            }
        }

        public static void CreateDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void CreateFile(string filePath)
        {
          
                if (!IsExistFile(filePath))
                {
                    CreateDirectory(GetDirectoryFromFilePath(filePath));
                    var  stream =FileStream.Synchronized(new FileStream(filePath, FileMode.OpenOrCreate));
                    stream.Flush();
                    stream.Close();
                }
        }

        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    CreateDirectory(GetDirectoryFromFilePath(filePath));
                    FileInfo info = new FileInfo(filePath);
                    using (FileStream stream = info.Create())
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
            }
        }

        public static void CreateFile(string filePath, string text)
        {
            CreateFile(filePath, text, Encoding.UTF8);
        }

        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                if (!IsExistFile(filePath))
                {
                    CreateDirectory(GetDirectoryFromFilePath(filePath));
                    FileInfo info = new FileInfo(filePath);
                    using (FileStream stream = info.Create())
                    {
                        using (StreamWriter writer = new StreamWriter(stream, encoding))
                        {
                            writer.Write(text);
                            writer.Flush();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static byte[] FileToBytes(string filePath)
        {
            int fileSize = GetFileSize(filePath);
            byte[] buffer = new byte[fileSize];
            FileInfo info = new FileInfo(filePath);
            using (FileStream stream = info.Open(FileMode.Open))
            {
                stream.Read(buffer, 0, fileSize);
                return buffer;
            }
        }

        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        public static string FileToString(string filePath, Encoding encoding)
        {
            string str;
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                str = reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                reader.Close();
            }
            return str;
        }

        public static string[] GetDirectories(string directoryPath)
        {
            string[] directories;
            try
            {
                directories = Directory.GetDirectories(directoryPath);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return directories;
        }

        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] strArray;
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                strArray = Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return strArray;
        }

        public static string GetDirectoryFromFilePath(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Directory.FullName;
        }

        public static string GetExtension(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Extension;
        }

        public static string GetFileName(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Name;
        }

        public static string GetFileNameNoExtension(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return info.Name.Split(new char[] { '.' })[0];
        }

        public static string[] GetFileNames(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }

        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            string[] strArray;
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                strArray = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (IOException exception)
            {
                throw exception;
            }
            return strArray;
        }

        public static int GetFileSize(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return (int) info.Length;
        }

        public static double GetFileSizeByKB(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return ConvertProvider.ToDouble<double>(ConvertProvider.ToDouble<long>(info.Length) / 1024.0, 1);
        }

        public static double GetFileSizeByMB(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            return ConvertProvider.ToDouble<double>((ConvertProvider.ToDouble<long>(info.Length) / 1024.0) / 1024.0, 1);
        }

        public static int GetLineCount(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                int num = 0;
                while (true)
                {
                    if (reader.ReadLine() == null)
                    {
                        break;
                    }
                    num++;
                }
                return num;
            }
        }

        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                if (GetFileNames(directoryPath).Length > 0)
                {
                    return false;
                }
                if (GetDirectories(directoryPath).Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void Move(string sourceFilePath, string descFilePath)
        {
            if (IsExistFile(sourceFilePath))
            {
                try
                {
                    CreateDirectory(GetDirectoryFromFilePath(descFilePath));
                    File.Move(sourceFilePath, descFilePath);
                }
                catch
                {
                }
            }
        }

        public static void MoveToDirectory(string sourceFilePath, string descDirectoryPath)
        {
            if (IsExistFile(sourceFilePath))
            {
                try
                {
                    string str2;
                    string fileName = GetFileName(sourceFilePath);
                    CreateDirectory(descDirectoryPath);
                    if (IsExistFile(descDirectoryPath + @"\" + fileName))
                    {
                        DeleteFile(descDirectoryPath + @"\" + fileName);
                    }
                    if (!descDirectoryPath.EndsWith(@"\"))
                    {
                        str2 = descDirectoryPath + @"\" + fileName;
                    }
                    else
                    {
                        str2 = descDirectoryPath + fileName;
                    }
                    File.Move(sourceFilePath, str2);
                }
                catch
                {
                }
            }
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer2;
            try
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, ConvertProvider.ToInt32<long>(stream.Length));
                buffer2 = buffer;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                stream.Close();
            }
            return buffer2;
        }

        public static void WriteText(string filePath, string text)
        {
            WriteText(filePath, text, Encoding.UTF8);
        }

        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            File.WriteAllText(filePath, text, encoding);
        }
    }
}

