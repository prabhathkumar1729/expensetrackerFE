window.StackedColumnChart = async (tableData) => {

    google.charts.load("current", { packages: ['corechart'] });
    google.charts.setOnLoadCallback(drawChart);
    function drawChart() {
        var data = google.visualization.arrayToDataTable(tableData);

        var options = {
            width: 600,
            height: 400,
            legend: { position: 'top', maxLines: 3 },
            bar: { groupWidth: '75%' },
            isStacked: true,
        };
        var chart = new google.visualization.ColumnChart(document.getElementById('stackedColumncontainer'));
        chart.draw(data, options);
    }
}