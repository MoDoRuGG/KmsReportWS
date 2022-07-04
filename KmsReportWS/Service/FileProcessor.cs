using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using NLog;

namespace KmsReportWS.Service
{
    public class FileProcessor
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void UploadFile(byte[] bytes, string fileName, string filial)
        {
            string finalPath = HostingEnvironment.MapPath("~/scan/") + filial + "/" + fileName;
            try
            {
                using var fs = new FileStream(finalPath, FileMode.Create);
                using var ms = new MemoryStream(bytes);
                ms.WriteTo(fs);
                ms.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error saving file: {finalPath}, error text: {ex.Message}");
                throw;
            }
        }


        public byte[] DownloadFile(string fileName, string filial)
        {
            string finalPath;
            if (!string.IsNullOrEmpty(filial))
            {
                finalPath = HostingEnvironment.MapPath("~/scan/") + filial + "/" + fileName;
            }
            else
            {
                finalPath = HostingEnvironment.MapPath("~/client-update/") + fileName;
            }

            byte[] data;
            using var fStream = new FileStream(finalPath, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fStream);
            var fInfo = new FileInfo(finalPath);
            long numBytes = fInfo.Length;

            data = br.ReadBytes((int)numBytes);
            br.Close();

            return data;
        }

        public byte[] DownloadDllFile(string fileName)
        {
           
            byte[] data;
            using var fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fStream);
            var fInfo = new FileInfo(fileName);
            long numBytes = fInfo.Length;

            data = br.ReadBytes((int)numBytes);
            br.Close();

            return data;
        }


        public void UploadXmlDynamicFile(byte[] bytes, string fileName)
        {
            string finalPath = HostingEnvironment.MapPath("~/dynamic-template/") + fileName;

            try
            {
                using var fs = new FileStream(finalPath, FileMode.Create);
                using var ms = new MemoryStream(bytes);
                ms.WriteTo(fs);
                ms.Close();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error saving file: {finalPath}, error text: {ex.Message}");
                throw;
            }
        }

        public string ReadXmlDynmaicFile(string fileName)
        {
            string finalPath = HostingEnvironment.MapPath("~/dynamic-template/") + fileName;
            try
            {
                return File.ReadAllText(finalPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error read XmlFile: {finalPath}, error text: {ex.Message}");
                throw;
            }

        }

        public List<string> GetDllFileNames()
        {
            List<string> filesNames = new List<string>();
            try
            {
                filesNames = Directory.GetFiles(HostingEnvironment.MapPath("~/client-update/main")).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Ошибка получения имён DLL файлов");
                throw;
            }

            return filesNames;
        }
    }
}