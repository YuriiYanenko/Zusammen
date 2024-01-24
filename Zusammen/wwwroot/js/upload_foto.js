function uploadPhoto() {
    var fileInput = document.getElementById('fileInput');
    var file = fileInput.files[0];

    if (file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var imageContainer = document.getElementById('imageContainer');
            imageContainer.innerHTML = `<img src="${e.target.result}" alt="Uploaded Photo" style="max-width: 100%; max-height: 300px;" />`;
        };

        reader.readAsDataURL(file);
    } else {
        alert('Оберіть файл.');
    }
}