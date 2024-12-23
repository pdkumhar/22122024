using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using VidConverter.Models;

namespace VidConverter.Controllers
{
    public class HomeController : Controller
    {
        // GET: Display the main page where the user can upload a file
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handle the file upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
            {
                ViewBag.ErrorMessage = "No file selected. Please choose a file to upload.";
                return View("Index");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, videoFile.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                // Pass the uploaded file to the same view for conversion
                ViewBag.FileName = videoFile.FileName;
                return View("Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Upload Error: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while uploading the file.";
                return View("Index");
            }
        }

        // POST: Handle the video conversion
        public IActionResult Convert(string fileName, string targetFormat)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string inputFilePath = Path.Combine(uploadsFolder, fileName);
            string outputFileName = Path.GetFileNameWithoutExtension(fileName) + "." + targetFormat;
            string outputFilePath = Path.Combine(uploadsFolder, outputFileName);

            // *** Check if the source and target formats are the same ***
            if (Path.GetExtension(fileName).TrimStart('.').ToLower() == targetFormat.ToLower())
            {
                ViewBag.ErrorMessage = "Source and target formats are the same. Please choose a different format.";
                ViewBag.FileName = fileName; // Set the uploaded file name
                return View("Index");
            }

            string ffmpegExePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ffmpeg", "ffmpeg.exe");

            if (!System.IO.File.Exists(ffmpegExePath))
            {
                return BadRequest("FFmpeg executable not found.");
            }

            if (!System.IO.File.Exists(inputFilePath))
            {
                return BadRequest("Input file not found.");
            }

            try
            {
                if (System.IO.File.Exists(outputFilePath))
                {
                    System.IO.File.Delete(outputFilePath);
                }

                string ffmpegArguments = $"-i \"{inputFilePath}\" \"{outputFilePath}\"";

                // Add specific arguments for different formats if needed
                if (targetFormat == "wmv")
                {
                    ffmpegArguments = $"-i \"{inputFilePath}\" -c:v wmv2 -b:v 1000k -c:a wmav2 -b:a 192k \"{outputFilePath}\"";
                }
                else if (targetFormat == "avi")
                {
                    ffmpegArguments = $"-i \"{inputFilePath}\" -c:v libx264 -c:a aac \"{outputFilePath}\"";
                }
                else if (targetFormat == "mp4")
                {
                    ffmpegArguments = $"-i \"{inputFilePath}\" -c:v libx264 -c:a aac \"{outputFilePath}\"";
                }

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegExePath,
                    Arguments = ffmpegArguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                string output;
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    output = process.StandardError.ReadToEnd(); // FFmpeg logs to StandardError
                    process.WaitForExit();
                }

                if (!System.IO.File.Exists(outputFilePath))
                {
                    Debug.WriteLine($"FFmpeg Error: {output}");
                    return StatusCode(500, "Video conversion failed.");
                }

                // Instead of redirecting, pass the download file name to the view
                ViewBag.DownloadFileName = outputFileName;
                return View("Index"); // Stay on the same page, showing the download link
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred during video conversion.");
            }
        }


        // Download converted file
        public IActionResult Download(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Return the file content as a download
                return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", fileName);
            }

            //  If the file doesn't exist, return a 404 error
            return NotFound();
        }

    }
}
