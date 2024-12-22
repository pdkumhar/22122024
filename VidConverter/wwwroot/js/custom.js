// Get references to DOM elements
const dropArea = document.getElementById("drag-and-drop-area");
const fileInput = document.getElementById("videoFile");
const uploadBtn = document.getElementById("uploadBtn");
const fileNameDisplay = document.getElementById("fileNameDisplay");
const selectFileBtn = document.getElementById("selectFileBtn");
const progressBarContainer = document.getElementById("progressBarContainer");
const videoDetails = document.getElementById("videoDetails");
const estimatedTime = document.getElementById("estimatedTime");

// Initial setup: hide elements that are not needed initially
uploadBtn.style.display = "none";
videoDetails.style.display = "none";
estimatedTime.style.display = "none";

// Event listener for drag over
dropArea.addEventListener("dragover", (event) => {
    event.preventDefault();
    dropArea.style.backgroundColor = "#f0f0f0";  // Change background on drag over
});

// Event listener for drag leave
dropArea.addEventListener("dragleave", () => {
    dropArea.style.backgroundColor = "";  // Reset background color on drag leave
});

// Handle file drop
dropArea.addEventListener("drop", (event) => {
    event.preventDefault();
    dropArea.style.backgroundColor = "";  // Reset background color on drop
    const file = event.dataTransfer.files[0];  // Get dropped file
    if (file) {
        processFile(file);  // Process the dropped file
    }
});

// Trigger file input click when "Select File" button is clicked
selectFileBtn.addEventListener("click", () => {
    fileInput.click();
});

// Handle file selection from file input
fileInput.addEventListener("change", () => {
    const file = fileInput.files[0];  // Get selected file
    if (file) {
        processFile(file);  // Process the selected file
    }
});

// Function to process and display file details
function processFile(file) {
    const fileName = file.name;
    const fileSize = (file.size / 1024 / 1024).toFixed(2); // Convert size to MB
    const fileExtension = fileName.split('.').pop().toLowerCase();

    // Update the file name display and show upload button
    fileNameDisplay.textContent = `Selected file: ${fileName}`;
    uploadBtn.style.display = "inline";
    videoDetails.style.display = "block"; // Show video details section

    // Display initial file details with placeholder for duration
    videoDetails.innerHTML = `
        <p><strong>File Name:</strong> ${fileName}</p>
        <p><strong>File Size:</strong> ${fileSize} MB</p>
        <p><strong>File Extension:</strong> ${fileExtension}</p>
        <p><strong>Video Duration:</strong> Loading...</p>
    `;

    // Estimate conversion time (2 seconds per MB)
    const conversionTime = Math.ceil(fileSize * 2);
    estimatedTime.style.display = "block";
    estimatedTime.textContent = `Approximate conversion time: ${conversionTime} seconds`;

    // Create video element to extract duration
    const videoElement = document.createElement("video");
    videoElement.preload = "metadata";
    videoElement.src = URL.createObjectURL(file);

    // Handle metadata load
    videoElement.onloadedmetadata = () => {
        const duration = videoElement.duration.toFixed(2); // Duration in seconds
        videoDetails.innerHTML = `
            <p><strong>File Name:</strong> ${fileName}</p>
            <p><strong>File Size:</strong> ${fileSize} MB</p>
            <p><strong>File Extension:</strong> ${fileExtension}</p>
            <p><strong>Video Duration:</strong> ${duration} seconds</p>
        `;
        URL.revokeObjectURL(videoElement.src); // Free memory
    };

    // Handle errors while loading video metadata
    videoElement.onerror = () => {
        videoDetails.innerHTML = `
            <p><strong>File Name:</strong> ${fileName}</p>
            <p><strong>File Size:</strong> ${fileSize} MB</p>
            <p><strong>File Extension:</strong> ${fileExtension}</p>
            <p><strong>Video Duration:</strong> Unable to retrieve duration</p>
        `;
    };
}

// Form submission handler for showing progress bar
if (convertForm) {
    convertForm.addEventListener("submit", (event) => {
        event.preventDefault(); // Prevent immediate form submission
        progressBarContainer.style.display = "block"; // Show progress bar

        // Simulate a delay before actual form submission
        setTimeout(() => {
            convertForm.submit(); // Submit the form after showing progress
        }, 100);
    });
}

// Hide progress bar after conversion is complete
window.addEventListener("load", () => {
    const downloadLink = document.querySelector(".alert-info a");
    if (downloadLink) {
        progressBarContainer.style.display = "none"; // Hide progress bar
    }
});
