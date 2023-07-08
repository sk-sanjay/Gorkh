using Application.Dtos;
using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private enum FileFormat { jpeg, jpeg2, png, bmp, gif, tiff, tiff2, pdf, officenew, officeold, corrupt, unknown }
        private static int AllowedFileSize;
        private static int AllowedVideoSize;
        //private static List<string> AllowedContentTypes;
        private static List<string> AllowedFileExtensions;
        private static List<string> AllowedVideoExtensions;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _config;
        public FileService(IWebHostEnvironment hostingEnvironment,
            IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _config = config;
            //AllowedContentTypes = _config.GetSection("AllowedContentTypes").GetChildren().Select(x => x.Value).ToList();
            AllowedFileExtensions = _config.GetSection("AllowedFileExtensions").GetChildren().Select(x => x.Value).ToList();
            AllowedVideoExtensions = _config.GetSection("AllowedVideoExtensions").GetChildren().Select(x => x.Value).ToList();
            AllowedFileSize = Convert.ToInt32(_config["AllowedFileSize"]);
            AllowedVideoSize = Convert.ToInt32(_config["AllowedVideoSize"]);
        }
        private static FileFormat GetFileExtType(byte[] bytes)
        {
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg or jpg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon
            var png = new byte[] { 137, 80, 78, 71 }; // png
            var tiff = new byte[] { 73, 73, 42 }; // TIFF
            var tiff2 = new byte[] { 77, 77, 42 }; // TIFF
            var officenew = new byte[] { 80, 75, 3, 4, 20 }; // OfficeNew
            var officeold = new byte[] { 208, 207, 17, 224, 161 }; // OfficeOld
            var corrupt = new byte[] { 77, 90, 144 };
            var bmp = Encoding.ASCII.GetBytes("BM"); // BMP
            var gif = Encoding.ASCII.GetBytes("GIF"); // GIF
            var pdf = Encoding.ASCII.GetBytes("%PDF-"); // pdf
            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return FileFormat.jpeg;
            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return FileFormat.jpeg2;
            if (png.SequenceEqual(bytes.Take(png.Length)))
                return FileFormat.png;
            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return FileFormat.bmp;
            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return FileFormat.gif;
            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return FileFormat.tiff;
            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return FileFormat.tiff2;
            if (pdf.SequenceEqual(bytes.Take(pdf.Length)))
                return FileFormat.pdf;
            if (corrupt.SequenceEqual(bytes.Take(corrupt.Length)))
                return FileFormat.corrupt;
            if (officenew.SequenceEqual(bytes.Take(officenew.Length)))
                return FileFormat.officenew;
            if (officeold.SequenceEqual(bytes.Take(officeold.Length)))
                return FileFormat.officeold;
            return FileFormat.unknown;
        }
        public bool CheckImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            var fileType = GetFileExtType(fileBytes);
            return AllowedFileExtensions.Contains(fileType.ToString());
        }
        public bool CheckFileSize(IFormFile file)
        {
            return file.Length <= AllowedFileSize;
        }
        //public bool CheckImageFiles(List<IFormFile> files)
        //{
        //    var filesAreImages = new List<bool>(files.Count);
        //    filesAreImages.AddRange(files.Select(CheckImageFile));
        //    return filesAreImages.All(x => x);
        //}
        public bool CheckVideoFile(IFormFile file)
        {
            var Extention = Path.GetExtension(file.FileName);
            return AllowedVideoExtensions.Contains(Extention);
        }
        public bool CheckVideoSize(IFormFile file)
        {
            return file.Length <= AllowedVideoSize;
        }
        //public bool CheckIfImageFile(IFormFile file)
        //{
        //    ////Check the image mime types
        //    //if (AllowedContentTypes.All(x => x != file.ContentType))
        //    //    return false;
        //    //  Check the image extension
        //    var fileExtension = Path.GetExtension(file.FileName);
        //    if (AllowedFileExtensions.All(x => x != fileExtension))
        //        return false;
        //    try
        //    {
        //        //Attempt to read the file and check the first bytes
        //        if (!file.OpenReadStream().CanRead) return false;
        //        //Check whether the image size exceeding the limit or not
        //        if (file.Length > AllowedFileSize) return false;
        //        var buffer = new byte[AllowedFileSize];
        //        file.OpenReadStream().Read(buffer, 0, AllowedFileSize);
        //        var content = Encoding.UTF8.GetString(buffer);
        //        if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
        //            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
        //            return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public bool CheckIfImageFileList(List<IFormFile> files)
        //{
        //    foreach (var file in files)
        //    {
        //        //Check the image mime types
        //        if (AllowedContentTypes.All(x => x != file.ContentType))
        //            return false;
        //        //  Check the image extension
        //        var fileExtension = Path.GetExtension(file.FileName);
        //        if (AllowedFileExtensions.All(x => x != fileExtension))
        //            return false;
        //        try
        //        {
        //            //Attempt to read the file and check the first bytes
        //            if (!file.OpenReadStream().CanRead) return false;
        //            //Check whether the image size exceeding the limit or not
        //            if (file.Length > AllowedFileSize) return false;
        //            var buffer = new byte[AllowedFileSize];
        //            file.OpenReadStream().Read(buffer, 0, AllowedFileSize);
        //            var content = Encoding.UTF8.GetString(buffer);
        //            if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
        //                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
        //                return false;
        //        }
        //        catch (Exception)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        public async Task<string> SaveFileAsync(FileUploadDTO FileUploadDto)
        {
            //check to see if folder exists if not create it
            FolderCreator(FileUploadDto.FilePath);

            var filename = FileUploadDto.ChangeName ? $"{Guid.NewGuid():N}{Path.GetExtension(FileUploadDto.UploadedFile.FileName)}" : FileUploadDto.UploadedFile.FileName;
            var relativepath = $"{FileUploadDto.FilePath}\\{filename}";
            var physicalPath = $"{_hostingEnvironment.WebRootPath}{relativepath}";

            if (!FileUploadDto.ChangeDimensions)
            {
                var fileCreated = await CreateFile(FileUploadDto.UploadedFile, physicalPath);
                if (fileCreated && !string.IsNullOrEmpty(FileUploadDto.ReturnValue))
                    return FileUploadDto.ReturnValue == "name" ? filename : relativepath;
            }

            //check if file is a image
            if (!CheckImageFile(FileUploadDto.UploadedFile)) return null;

            //Create file at temp path
            var relativetemppath = $"\\img\\temp\\{filename}";
            var tempPath = $"{_hostingEnvironment.WebRootPath}{relativetemppath}";
            var tempFileCreated = await CreateFile(FileUploadDto.UploadedFile, tempPath);
            if (!tempFileCreated) return null;

            //Save resized image at physical path
            var resized = new Bitmap(FileUploadDto.Width, FileUploadDto.Height);
            var graphics = Graphics.FromImage(resized);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceCopy;
            var imageRectangle = new Rectangle(0, 0, FileUploadDto.Width, FileUploadDto.Height);
            var image = Image.FromFile(tempPath);
            graphics.DrawImage(image, imageRectangle);
            resized.Save(physicalPath, image.RawFormat);
            image.Dispose();
            resized.Dispose();

            //Delete the temp file
            DeleteFile(relativetemppath);
            if (File.Exists(physicalPath) && !string.IsNullOrEmpty(FileUploadDto.ReturnValue))
                return FileUploadDto.ReturnValue == "name" ? filename : relativepath;

            return null;
        }

        private static async Task<bool> CreateFile(IFormFile file, string path)
        {
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream).ConfigureAwait(false);
            await stream.DisposeAsync();
            //return File.Exists(path);
            return File.Exists(path);
        }

        public async Task<string> SaveFile2(IFormFile file, string path)
        {
            await using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream).ConfigureAwait(false);
            await stream.DisposeAsync();
            //return File.Exists(path);
            return file.FileName;
        }


        public async Task<string> SaveImageAsync(string path, IFormFile file)
        {
            //check to see if folder exists if not create it
            FolderCreator(path);
            var filename = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            path = $"{_hostingEnvironment.WebRootPath}{path}\\{filename}";
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
        }

        public async Task<List<string>> SaveImageAsync(string path, List<IFormFile> files)
        {
            //check to see if folder exists if not create it
            //FolderCreator(path);
            List<string> fileNames = new List<string>();
            foreach (var file in files)
            {
                var filename = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                string pathN = $"{_hostingEnvironment.WebRootPath}{path}\\{filename}";
                string pathW = $"{_hostingEnvironment.WebRootPath}\\img\\logo-surplus.png";
                //using (var stream = new FileStream(pathN, FileMode.Create))
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    // Add watermark
                    var watermarkedStream = new MemoryStream();
                    using (var img = Image.FromStream(stream))
                    {
                        using (var graphic = Graphics.FromImage(img))
                        {
                            //var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
                            //var color = Color.FromArgb(128, 255, 255, 255);
                            //var brush = new SolidBrush(color);
                            var point = new Point((img.Width - 200)/2, (img.Height - 84)/2);
                            using (var waterimagestream = new FileStream(pathW, FileMode.Open))
                            {
                                using (var watermarkimgage = Image.FromStream(waterimagestream))
                                {
                                    graphic.DrawImage(watermarkimgage, point);
                                    img.Save(watermarkedStream, ImageFormat.Png);
                                }
                            }
                        }
                        img.Save(pathN);
                    }
                    fileNames.Add(filename);
                }
            }

            return fileNames;
        }

        //public async Task<string> SaveFileAsync(string path, IFormFile file, bool changename, string nameorpath)
        //{
        //    //check to see if folder exists if not create it
        //    FolderCreator(path);
        //    var filename = changename ? $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}" : file.FileName;
        //    var relativepath = $"{path}\\{filename}";
        //    path = $"{_hostingEnvironment.WebRootPath}{relativepath}";
        //    await using var stream = new FileStream(path, FileMode.Create);
        //    await file.CopyToAsync(stream).ConfigureAwait(false);
        //    await stream.DisposeAsync();
        //    var returnvalue = string.Empty;
        //    if (!string.IsNullOrEmpty(nameorpath))
        //        returnvalue = nameorpath == "name" ? filename : relativepath;
        //    return returnvalue;
        //}
        //public async Task<string> SaveFileAsync(string path, IFormFile file)
        //{
        //    //check to see if folder exists if not create it
        //    FolderCreator(path);
        //    var filename = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        //    path = $"{_hostingEnvironment.WebRootPath}{path}\\{filename}";
        //    await using var stream = new FileStream(path, FileMode.Create);
        //    await file.CopyToAsync(stream).ConfigureAwait(false);
        //    return filename;
        //}
        public async Task<string> SaveFileAsync(string path, Stream stream, string name, string extension)
        {
            //check to see if folder exists if not create it
            FolderCreator(path);
            var filename = $"{name}.{extension}";
            path = $"{_hostingEnvironment.WebRootPath}{path}\\{filename}";
            await using var fileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fileStream).ConfigureAwait(false);
            return filename;
        }
        //public async Task<List<string>> SaveFileAsync(string path, List<IFormFile> files)
        //{
        //    var filenames = new List<string>(files.Count);
        //    foreach (var file in files)
        //        filenames.Add(await SaveFileAsync(path, file).ConfigureAwait(false));
        //    return filenames;
        //}
        private void FolderCreator(string path)
        {
            var path_to_check = _hostingEnvironment.WebRootPath;
            var folders = path.Split(@"\");
            foreach (var item in folders)
            {
                path_to_check += item;
                if (!Directory.Exists(path_to_check))
                    Directory.CreateDirectory(path_to_check);
                path_to_check += @"\";
            }
        }
        public bool DeleteFile(string path)
        {
            bool deleted;
            var folderOK = true;
            //check to see if folder exists
            var path_to_check = _hostingEnvironment.WebRootPath;
            var folders = path.Split(@"\");
            try
            {
                for (var i = 0; i < folders.Length - 1; i++)
                {
                    path_to_check += folders[i];
                    if (!Directory.Exists(path_to_check))
                        folderOK = false;
                    path_to_check += @"\";
                }
                if (folderOK)
                    File.Delete($"{_hostingEnvironment.WebRootPath}{path}");
                deleted = true;
            }
            catch (Exception)
            {
                deleted = false;
            }
            return deleted;
        }
        public byte[] Download(string path, string filename)
        {
            var filepath = $"{_hostingEnvironment.WebRootPath}{path}{filename}";
            try
            {
                var fileBytes = File.ReadAllBytes(filepath);
                return fileBytes;
            }
            catch (Exception)
            {
                Console.WriteLine();
                return null;
            }
        }

        //public string CopyFile(string sourcepath, string sourcename, string destinationpath, string destinationname, bool overwrite)
        //{
        //    var source = $"{_hostingEnvironment.WebRootPath}{sourcepath}{sourcename}";
        //    var destination = $"{_hostingEnvironment.WebRootPath}{destinationpath}{destinationname}";
        //    File.Copy(source, destination, overwrite);
        //    return $"{_hostingEnvironment.WebRootPath}{destinationpath}";
        //}

        public string GetPhysicalPath(string folderpath)
        {
            var path = $"{_hostingEnvironment.WebRootPath}";
            if (!string.IsNullOrEmpty(folderpath))
                path = $"{_hostingEnvironment.WebRootPath}{folderpath}";
            return path;
        }

        public void ExtractZipFile(string relativepath, IFormFile file)
        {
            var path = $"{_hostingEnvironment.WebRootPath}{relativepath}";
            using var archive = new ZipArchive(file.OpenReadStream());
            foreach (var entry in archive.Entries)
            {
                if (!string.IsNullOrEmpty(Path.GetExtension(entry.FullName)))
                    entry.ExtractToFile(Path.Combine(path, entry.FullName), true);
                else
                    Directory.CreateDirectory(Path.Combine(path, entry.FullName));
            }
        }
        //public async Task<string> SaveFileAsync(string path, string fileName, byte[] content)
        //{
        //    //check to see if folder exists if not create it
        //    FolderCreator(path);
        //    var filename = $"{Guid.NewGuid():N}{Path.GetExtension(fileName)}";
        //    path = $"{_hostingEnvironment.WebRootPath}{path}\\{filename}";
        //    await File.WriteAllBytesAsync(path, content);
        //    return filename;
        //}
    }
}
