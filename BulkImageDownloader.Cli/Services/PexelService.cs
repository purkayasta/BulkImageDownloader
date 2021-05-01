﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BulkImageDownloader.Cli.Interfaces;
using BulkImageDownloader.Cli.ViewModels;
using PexelsDotNetSDK.Api;

namespace BulkImageDownloader.Cli.Services
{
    public class PexelService : BaseService, IPexelService
    {
        private readonly PexelsClient _pexelsClient;
        public PexelService(IHttpClientFactory httpClientFactory, string accessKey) : base(httpClientFactory, ClientEnum.Pexels)
        {
            _pexelsClient = new PexelsClient(accessKey);
        }

        public async Task InitiateDownloadAsync(WallpaperModel wallpaperModel)
        {
            Console.WriteLine("⏬ Downloading .... ");
            var imageInfos = wallpaperModel.ImageInfos;
            var pexelsDirectory = $"{wallpaperModel.DirectoryLocation}/Pexels/";
            Directory.CreateDirectory(pexelsDirectory);

            int progress = 0;
            int totalCount = wallpaperModel.NumberOfImages;

            foreach (var image in imageInfos)
            {
                var responses = await GetContentAsync(image.Url);
                await SaveAsync(responses, $"{pexelsDirectory}/{image.Name}");

                progress++;
                Console.Write($"\r {progress} | {totalCount}");
            }

            Console.WriteLine("");
            Console.WriteLine("⏬ Downloaded ....  ✔ 💹");
        }

        public async Task<List<ImageInfo>> SearchPhotosByNameAsync(string name, int count)
        {
            var photoPage = await _pexelsClient.SearchPhotosAsync(name, pageSize: count);

            List<ImageInfo> imagesInfo = new();

            if (photoPage == null)
                return imagesInfo;

            foreach (var photo in photoPage.photos)
            {
                imagesInfo.Add(new ImageInfo
                {
                    Url = photo.source.landscape,
                    Name = photo.id.ToString() + ".jpg"
                });
            }
            return imagesInfo;
        }

        public async Task<List<ImageInfo>> GetCurratedImagesAsync(int count)
        {
            var photoPage = await _pexelsClient.CuratedPhotosAsync(pageSize: count);

            List<ImageInfo> imagesInfo = new();

            if (photoPage == null)
                return imagesInfo;

            foreach (var photo in photoPage.photos)
            {
                imagesInfo.Add(new ImageInfo
                {
                    Url = photo.source.landscape,
                    Name = photo.id.ToString() + ".jpg"
                });
            }
            return imagesInfo;
        }
    }
}
