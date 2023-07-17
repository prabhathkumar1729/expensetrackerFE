window.drawChart = (chartData) => {
    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {
        var data = google.visualization.arrayToDataTable(chartData);

        var options = {
            isStacked: true,
            height: 300,
            legend: { position: 'top', maxLines: 3 },
            vAxis: { minValue: 0 },
        };

        var chart = new google.visualization.ColumnChart(document.getElementById('chart'));
        chart.draw(data, options);
    }
};
