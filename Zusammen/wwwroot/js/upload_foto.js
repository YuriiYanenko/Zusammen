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
