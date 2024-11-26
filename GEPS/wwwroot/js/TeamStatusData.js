document.addEventListener("DOMContentLoaded", function () {
    // Team status hücresini seç
    const teamStatusElement = document.querySelector("#teamTableBody td:nth-child(2) p");

    if (teamStatusElement) {
        const status = teamStatusElement.textContent.trim();

        
        switch (status) {
            case "READY TO EVALUATE":
                teamStatusElement.style.color = "blue";
                teamStatusElement.style.fontWeight = "bold";
                teamStatusElement.textContent = " Ready to Evaluate";
                break;

            case "NO RESULT":
                teamStatusElement.style.color = "red";
                teamStatusElement.style.fontWeight = "bold";
                teamStatusElement.textContent = "❌ No Result";
                break;

            case "RESULT AVAILABLE":
                teamStatusElement.style.color = "green";
                teamStatusElement.style.fontWeight = "bold";
                teamStatusElement.textContent = "✅ Result Available";
                break;

            default:
                teamStatusElement.style.color = "black";
                teamStatusElement.textContent = "Unknown Status";
                break;
        }
    }
});