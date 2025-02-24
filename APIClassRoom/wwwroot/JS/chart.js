window.initializeCharts = (data) => {
    const labels = ["Team 1", "Team 2", "Team 3"];

    // Bar Chart
    const ctxBar = document.getElementById("barChart").getContext("2d");
    new Chart(ctxBar, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [{
                label: "Progress (%)",
                data: data,
                backgroundColor: ["#4CAF50", "#FF9800", "#2196F3"]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: { beginAtZero: true, max: 100 }
            }
        }
    });

    // Doughnut Chart
    const ctxCircle = document.getElementById("circleChart").getContext("2d");
    new Chart(ctxCircle, {
        type: "doughnut",
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: ["#4CAF50", "#FF9800", "#2196F3"]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
};
