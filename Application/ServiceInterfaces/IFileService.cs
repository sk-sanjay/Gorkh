﻿using Application.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Application.ServiceInterfaces
{
    public interface IFileService
    {
        bool CheckVideoFile(IFormFile file);
        bool CheckVideoSize(IFormFile file);
        bool CheckImageFile(IFormFile file);
        bool CheckFileSize(IFormFile file);
        //bool CheckImageFiles(List<IFormFile> files);
        Task<string> SaveFileAsync(FileUploadDTO FileUploadDto);
        //Task<string> SaveFileAsync(string path, IFormFile file, bool changename, string nameorpath);
        Task<string> SaveFileAsync(string path, Stream stream, string name, string extension);
        //Task<string> SaveFileAsync(string path, IFormFile file);
        //Task<List<string>> SaveFileAsync(string path, List<IFormFile> files);
        bool DeleteFile(string path);
        byte[] Download(string path, string filename);
        //string CopyFile(string sourcepath, string sourcename, string destinationpath, string destinationname, bool overwrite);
        string GetPhysicalPath(string relativepath);
        void ExtractZipFile(string relativepath, IFormFile file);
        //Task<string> SaveFileAsync(string path, string fileName, byte[] content);
        Task<string> SaveFile2(IFormFile file, string path);
        Task<string> SaveImageAsync(string path, IFormFile file);
        Task<List<string>> SaveImageAsync(string path, List<IFormFile> files);

    }
}