﻿using System;
using System.IO;
using BulkImageDownloader.Cli.ViewModels;

namespace BulkImageDownloader.Cli.Menu
{
    public abstract class MenuBase
    {
        private readonly ClientEnum _wallpaperClient;
        public MenuBase(ClientEnum wallpaperClientEnum)
        {
            _wallpaperClient = wallpaperClientEnum;
        }
        public virtual int DownloadableImageCount { get; set; } = 10;
        public virtual string Tags { get; set; }
        public virtual string DownloadedDirectory { get; set; } = "Wallpapers/";
        public virtual string SpecialDownloadCountRules { get; set; }
        public virtual string SpecialTagRules { get; set; }
        public virtual int MaxDownloadLimit { get; set; } = 0;
        public abstract WallpaperModel Build();

        /// Needs Proper Validation
        /// <returns></returns>
        public void WallpaperCountSelector()
        {
            /* 
			 * Cannot Accept Unlimited Numbers
			 * Cannot Accept Negative Numbers
			 */
            Console.Write($"📸🎦 How many  images you want to downlaod - (Default: {DownloadableImageCount} {SpecialDownloadCountRules}): ");

            string input = Console.ReadLine();
            Console.WriteLine("");

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            if (int.TryParse(input, out int answer))
            {
                if (MaxDownloadLimit > 0)
                {
                    if (answer > MaxDownloadLimit)
                    {
                        Console.WriteLine("Max Download Limit Crossed! ❌❌");
                        Console.WriteLine("Please Provide a value within the range!! ");
                        WallpaperCountSelector();
                    }
                }

                if (answer > 0)
                {
                    DownloadableImageCount = answer;
                    return;
                }
            }

            Console.WriteLine($"You want to downlaod {answer} images from {_wallpaperClient} ? 😵😵");
            Console.WriteLine("Please provide a valid answer 🙏");
            WallpaperCountSelector();
        }

        /// Needs Proper Validation
        /// <returns></returns>
        public void WallpaperTypeSelector()
        {
            /* 
			 * Needs Proper Validation
			 * Cannot Accept Special Characters
			 * Only Accept Comma Separated Text
			 */
            Console.Write($"🌀🏁 Select Tags - (Default : {Tags}): {SpecialTagRules} ");
            string answer = Console.ReadLine();

            Console.WriteLine("");

            if (string.IsNullOrEmpty(answer))
                return;

            Tags = answer;
        }

        public void DirectoryLocationSelector()
        {
            Console.Write("📩 Where you want to download your images? (Default - Current Directory): ");
            var answer = Console.ReadLine();

            Console.WriteLine("");

            if (!string.IsNullOrEmpty(answer))
            {
                if (Directory.Exists(answer))
                {
                    var diretory = Directory.CreateDirectory(answer + "/" + DownloadedDirectory);

                    DownloadedDirectory = diretory.FullName;

                    return;
                }

                Console.WriteLine("Invalid Directory !");
                DirectoryLocationSelector();
            }
        }
    }
}
