const photos = document.querySelectorAll('.input-file');
const url = "https://localhost:5002/";
const editPhotoUrl = url + "profile/photo/edit";
photos.forEach(photo => {
    photo.addEventListener('change', (event) => {
        var image = document.getElementById("output");
        var file = document.getElementById("myFile");
        image.src = URL.createObjectURL(event.target.files[0]);
        var formData = new FormData();
        formData.append("files", file.files[0]);
        editPhoto(formData);
    })
})
async function editPhoto(formData) {
    const result = await fetch(`${editPhotoUrl}`, {
        method: 'PATCH',
        body: formData
    })

    const response = await result.json();
    return response
}