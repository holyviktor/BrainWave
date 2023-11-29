const photos = document.querySelectorAll('.input-file');

photos.forEach(photo => {
    photo.addEventListener('change', (event) => {
        var image = document.getElementById("output");
        image.src = URL.createObjectURL(event.target.files[0]);
    })
})