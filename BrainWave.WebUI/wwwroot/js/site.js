const articleContent = document.querySelector(".articles-content")
const articles = articleContent.querySelectorAll(".article")
const url = "https://localhost:6001/"
const likeUrl = url + "articles/likes"
const saveUrl = url + "articles/savings"
const commentAddUrl = url + "articles/comment"
const commentDeleteUrl = url + "articles/comment/delete"
const cookie = document.cookie.split("; ")
const accessToken = cookie.find(value => value.startsWith('token'))
let token = ''
if (accessToken) {
    token = accessToken.split("=")[1]
}


articles.forEach(article => {
    const button = article.querySelector(".likes button")
    const amount = article.querySelector(".likes p")
    const commentButton = article.querySelector('.sender-comment')
    const commentInput = article.querySelector('.comment-input')
    const commentAmount = article.querySelector('.comments p')
    const commentSection = article.querySelector('.comments-list') 
    const id = article.getAttribute("target")

    const commentPattern = (comment, avatar, name) => `<div class="comment d-flex justify-content-between pb-1" targetComment="${comment.id}">
                                    <div>
                                    <div class="author-comment d-flex justify-content-start align-items-center">

                                        <img class="d-block" src="/media/${avatar}" alt="avatar">
                                        <p class="m-0">${name}</p>
                                    </div>
                                    <div>
                                        ${comment.text}
                                    </div>
                                    </div>
                                    <button class="delete-button">Delete</button> 
                                </div>`

    addDeleteListeners(commentSection, id, commentAmount)


    commentButton.addEventListener("click", () => {
     
        patchComment(+id, commentInput.value)
            .then((response) => {
                commentAmount.innerText = +commentAmount.innerText + 1
                commentSection.innerHTML = commentPattern({
                    text: commentInput.value,
                    id: response.id
                },
                    avatar = response.user.photo,
                    name = response.user.name + ' ' + response.user.surname
                ) + commentSection.innerHTML
                commentInput.value = ""
            })
    })

    button.addEventListener("click", (e) => {
        e.preventDefault()
        const status = button.getAttribute("status")
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

function addDeleteListeners(commentsContainer, arcticleId, commentAmount) {

    commentsContainer.addEventListener("click", (e) => {
        const tag = e.target.tagName;
        const classN = e.target.className;
        console.log(tag, classN)
        if (tag.toLowerCase() !== 'button' || classN !== 'delete-button') return;

        const parent = e.target.parentElement

        const commentId = +parent.getAttribute("targetComment")
        console.log(commentId, parent);

        deleteComment(commentId, arcticleId)
            .then((a) => {
                if (!a.commentId) {
                    return
                }
                
                commentAmount.innerText = +commentAmount.innerText - 1
                commentsContainer.removeChild(parent)

            })
    })

}

async function patchLike(articleId, status) {
    const headers = new Headers({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
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
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    })
    const result = await fetch(`${commentAddUrl}`, {
        method: 'PATCH',
        body: JSON.stringify({ articleId, comment }),
        headers
    })

    const response = await result.json();
    return response
}

async function deleteComment(commentId, articleId) {
    const headers = new Headers({
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    })
    const result = await fetch(`${commentDeleteUrl}`, {
        method: 'DELETE',
        body: JSON.stringify({ articleId, commentId }),
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
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
    })
    const result = await fetch(`${saveUrl}`, {
        method: 'PATCH',
        body: JSON.stringify({ articleId, status }),
        headers
    })

    const response = await result.json();
    return response
}