/*function openFileExplorer() 
{
    document.getElementById('fileInput').click();
}

function previewImage() 
{
    var fileInput = document.getElementById('fileInput');
    var uploadedImageContainer = document.getElementById('uploadedImageContainer');
    var uploadedImage = document.getElementById('uploadedImage');

    var file = fileInput.files[0];
    var reader = new FileReader();

    reader.onload = function (e) 
    {
        uploadedImage.src = e.target.result;
        uploadedImageContainer.style.background = 'none';
    };

    if (file) 
    {
        reader.readAsDataURL(file);
    }
}*/
function main() {
    addListeners();
}
addEventListener('load',main);

function showInfo() {
    removeListeners();

    var modal = this.parentNode.getElementsByClassName('modal')[0];
    modal.style.display = 'block';
    var close = modal.getElementsByClassName('close')[0];
    close.addEventListener('click',hideInfo);
    document.getElementById('overlay').addEventListener('click',hideInfo);
}

function hideInfo() {
    var boxes = document.querySelectorAll('.modal');
    for (var i=0;i<boxes.length;i++) {
        boxes[i].style.display = 'none';
    }
    addListeners();
}

function addListeners() {
    var boxes = document.querySelectorAll('change-profile');
    for (var i=0;i<boxes.length;i++) {
        boxes[i].addEventListener('click',showInfo.bind(boxes[i]));
    }
    handleOverlay('none');
}

function removeListeners() {
    var boxes = document.querySelectorAll('change-profile');
    for (var i=0;i<boxes.length;i++) {
        boxes[i].removeEventListener('click',showInfo);
    }
    handleOverlay('block');
}

function handleOverlay(status) {
    var overlay = document.getElementById('overlay');
    overlay.style.display = status;
}
