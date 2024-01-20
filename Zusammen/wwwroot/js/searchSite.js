$(document).ready(function () {

    $("#filmSearchForm").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: 'ZusammenDb/GetFilms/',
            type: 'POST',
            dataType: 'json',
            data: $(this).serialize(),
            success: function (data) {
                displayResults(data);
            }
        });
    });

    function displayResults(data) {
        var resultsContainer = $("#results");
        resultsContainer.empty();

        if (data.length === 0) {
            resultsContainer.append("<p>Фільми не знайдені.</p>");
        }
        else {
            resultsContainer.append("<p>Результати пошука:</p>");
            resultsContainer.append("<ul>");
            data.forEach(function (movie) {
                resultsContainer.append("<li>" + movie.title + "</li>");
            });
            resultsContainer.append("</ul>");
        }
    }
});