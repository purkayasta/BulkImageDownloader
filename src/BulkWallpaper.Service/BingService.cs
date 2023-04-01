﻿using BingWallpaper.Contracts;
using BingWallpaper.Installer;
using BulkWallpaper.Utils;

namespace BulkImageDownloader.Service
{
	public class BingService
	{
		private static IBingWallpaperService? _wallpaperService = BingWallpaperInstaller.CreateService();

		public static async Task DownloadAsync(int download, string location)
		{
			var bingWallpaperSource = await _wallpaperService!.GetDailyWallpaperInfoAsync(download);

			if (bingWallpaperSource == null || !bingWallpaperSource.Any())
			{
				await Console.Out.WriteLineAsync("Cannot find any image description. Create a github issue please.");
				await Console.Out.WriteLineAsync("Github Issue: " + ContactDeveloper.GithubIssueBoardLink);
				await Console.Out.WriteLineAsync(Credits.SorryText);
			}

			foreach (var image in bingWallpaperSource!)
			{
				try
				{
					await Console.Out.WriteLineAsync("Title: " + image.Title);
					await Console.Out.WriteLineAsync("Copyright: " + image.CopyRight);
					await _wallpaperService.DownloadAsync(image.Url!, image.Title!.Trim(), "png", location);
				}
				catch (Exception)
				{
                    await Console.Out.WriteLineAsync("Something went wrong when downloading from bing!");
                    await Console.Out.WriteLineAsync("Create a issue in the github " + ContactDeveloper.GithubIssueBoardLink);
                }				
			}
		}
	}
}