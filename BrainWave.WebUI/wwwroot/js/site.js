const articleContent = document.querySelector(".articles-content")
const articles = articleContent.querySelectorAll(".article")
const likeUrl = "articles/likes"
const saveUrl = "articles/savings"
const commentAddUrl = "articles/comment"
const commentDeleteUrl = "articles/comment/delete"


articles.forEach(article => {
    const button = article.querySelector(".likes button")
    const amount = article.querySelector(".likes p")
    const commentButton = article.querySelector('.sender-comment')
    const commentInput = article.querySelector('.comment-input')
    const commentAmount = article.querySelector('.comments p')
    const commentSection = article.querySelector('.comments-list') 

    const commentPattern = (comment,  avatar = "/media/ava.jpg", name = "Name Surname") => `<div class="comment pb-1" targetComment="@comment.Id">

                                    <div class="author-comment d-flex justify-content-start align-items-center">

                                        <img class="d-block" src="${avatar}" alt="avatar">
                                        <p class="m-0">${name}</p>
                                    </div>
                                    <div>
                                        ${comment}
                                    </div>
                                </div>`

    commentButton.addEventListener("click", () => {
        const id = article.getAttribute("target")
 

        patchComment(+id, commentInput.value)
            .then(() => {
                commentAmount.innerText = +commentAmount.innerText + 1
                commentSection.innerHTML = commentPattern(commentInput.value) + commentSection.innerHTML
                commentInput.value = ""
            })
    })

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
                amount.innerText = response.countInteractions
                const second = image.getAttribute("second")
                image.setAttribute("second", image.getAttribute("src"))
                image.setAttribute("src", second)
            })
       
    })
})

async function patchLike(articleId, status) {
    const headers = new Headers({
        "Content-Type": "application/json"
    })
    const result = await fetch(`${likeUrl}`, {
        method: 'PATCH',
        body: JSON.stringify({ articleId, status }),
        headers
    })

    const response = await result.json();
    return response
}

async function patchComment(articleId, comment) {
    const headers = new Headers({
        "Content-Type": "application/json"
    })
    const result = await fetch(`${commentAddUrl}`, {
        method: 'PATCH',
        body: JSON.stringify({ articleId, comment }),
        headers
    })

    const response = await result.json();
    return response
}


articles.forEach(article => {
    const button = article.querySelector(".savings button")
    const amount = article.querySelector(".savings p")

    button.addEventListener("click", (e) => {
        e.preventDefault()
        const status = button.getAttribute("statusSave")
        const id = article.getAttribute("target")
        const image = button.querySelector("img")
        patchSave(id, status === "not-saved")
            .then(response => {
                if (status === "not-saved") {
                    button.setAttribute("statusSave", "saved")
                } else {
                    button.setAttribute("statusSave", "not-saved")
                }
                amount.innerText = response.countInteractions
                const second = image.getAttribute("secondSave")
                image.setAttribute("secondSave", image.getAttribute("src"))
                image.setAttribute("src", second)
            })

    })
})

async function patchSave(articleId, status) {
    const headers = new Headers({
        "Content-Type": "application/json"
    })
    const result = await fetch(`${saveUrl}`, {
        method: 'PATCH',
        body: JSON.stringify({ articleId, status }),
        headers
    })

    const response = await result.json();
    return response
}