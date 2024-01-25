function toggleContainers() 
{
    var photoContainer = document.getElementById('profileUs');
    var photoChangeContainer = document.getElementById('change-profileUs');
    var editButtonContainer = document.getElementById('editButtonContainer');
    var saveButtonContainer = document.getElementById('saveButtonContainer');
    var dataUsContainer = document.getElementById('dataUsContainer');

    if (photoContainer.style.display === 'block' || photoContainer.style.display === '') 
    {
        photoContainer.style.display = 'none';
        editButtonContainer.style.display = 'none';
        saveButtonContainer.style.display = 'block';
        photoChangeContainer.style.display = 'block';
        dataUsContainer.style.display = 'block'; 
    } else {
        photoContainer.style.display = 'block';
        editButtonContainer.style.display = 'block';
        saveButtonContainer.style.display = 'none';
        photoChangeContainer.style.display = 'none';
        dataUsContainer.style.display = 'none'; 
    }
}
function openFileExplorer() 
{
    document.getElementById('fileInput').click();
}

function previewImage() 
{
    var fileInput = document.getElementById('fileInput');
    var container = document.getElementById('profileUs');
    var uploadedImage = document.getElementById('uploadedImage1');

    var file = fileInput.files[0];
    var reader = new FileReader();

    reader.onload = function(e) 
    {
        container.style.background = 'none';
        uploadedImage.src = e.target.result;
    };

    reader.readAsDataURL(file);
}
function saveChanges() 
{
   
    location.reload();
}