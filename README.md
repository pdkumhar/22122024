Video Converter Web App

Overview

This is a web-based video converter application built using ASP.NET Core MVC Razor Pages. The application allows users to upload videos and convert them into various popular formats, such as MP4, AVI, MOV, WMV, and FLV. It features a user-friendly interface with drag-and-drop functionality and displays essential information like video size, duration, name, extension, and estimated conversion time.

Features

Upload Video: Supports drag-and-drop or file selection for video uploads.

Format Conversion: Convert videos to MP4, AVI, MOV, WMV, and FLV formats.

Video Information: Displays details such as:

Video size

Duration

File name

File extension

Expected conversion time

Responsive Design: Works seamlessly on desktop and mobile devices.

Screenshots

Home Page



Video Upload



Conversion Results



Getting Started

Prerequisites

.NET 8.0 SDK or later

Visual Studio 2022

FFmpeg: Ensure FFmpeg is installed and added to your system’s PATH.

Installation

Clone the repository:

git clone https://github.com/yourusername/video-converter.git
cd video-converter

Open the solution file in Visual Studio.

Restore NuGet packages:

dotnet restore

Update the FFmpeg path in the configuration file if necessary.

Run the application:

dotnet run

Open your browser and navigate to http://localhost:5000.

Project Structure

VideoConverter/
|-- Controllers/
|   |-- HomeController.cs
|-- Models/
|   |-- VideoModel.cs
|-- Views/
|   |-- Home/
|       |-- Index.cshtml
|       |-- Upload.cshtml
|-- wwwroot/
|   |-- css/
|   |-- js/
|-- Program.cs
|-- appsettings.json

Deployment

To deploy the application:

Publish the project in Visual Studio.

Host it on your preferred server or cloud platform (e.g., IIS, Azure, AWS).

Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.
