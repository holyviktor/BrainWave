const articleContent = document.querySelector(".articles-content")
const articles = articleContent.querySelectorAll(".article")
const likeUrl = "articles/likes"

articles.forEach(article => {
    const button = article.querySelector(".likes button")
    const amount = article.querySelector(".likes p")

    button.addEventListener("click", (e) => {
        e.preventDefault()
        const status = button.getAttribute("status")
        const id = article.getAttribute("target")
        const image = button.querySelector("img")
        patchLike(id, status === "not-liked")
            .then(response => {
                if (status === "not-liked") {
                    button.setAttribute("status", "liked")
                } else {
                    button.setAttribute("status", "not-liked")
                }
                amount.innerText = response.countLikes
                const second = image.getAttribute("second")
                image.setAttribute("second", image.getAttribute("src"))
                image.setAttribute("src", second)
            })
       
    })
})

async function patchLike(id, status) {
    const headers = new Headers({
        "Content-Type": "application/json"
    })
    const result = await fetch(`${likeUrl}/${id}`, {
        method: 'PATCH',
        body: JSON.stringify({ status }),
        headers
    })

    const response = await result.json();
    return response
}