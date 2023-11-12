# The social network website for scientists

## Description of the website

The social network website for posting and viewing scientific publications is an online resource that will be created to facilitate the exchange of scientific information and access to current research and scientific articles. The main goal of the site is to provide a convenient tool for posting and searching for scientific works, communication between scientists, and promoting the development of the scientific community.

The site supports the following roles: unauthorized user, authorized user, moderator and administrator.
## **Functionality**

* Creation and editing of publications: Authorized users are able to create new scientific publications on the site. Publications can be paid or free. Free posts are available to all users, and paid posts are only available to users who have paid the post. There should be the functionality of editing and deleting own publications.

* Display of publications: Publications must be displayed on the site as separate pages with all relevant information, including title, author, abstract, overall rating and text of the publication. It is necessary to ensure the possibility of commenting and feedback on publications by users.

* Payment for paid publications: Paid publications must be paid for viewing, for which you need to connect payment API.

* Recommendation of publications: Based on the user's previously viewed and liked publications, the system should recommend relevant articles to the user's taste and needs.

* Registration and authorization: The site must provide the ability to register new users. Users must be able to log in to the site using an email address and password.

* Filing a complaint about publication. Users can file a complaint about a publication if, in their opinion, it violates the rules of using the social network. At the same time, the moderator considers the complaints and decides whether the publication really violates the rules.

* Search and filtering system: The site must have the functionality of searching publications by keywords, categories, authors and other parameters. It should be possible to filter and sort the search results.

* Subscribe to an author: Users can subscribe to the profile of authors, which will allow viewing only the posts of authors to which the user is subscribed.

* Favorites and Activity Tracking: Users should be able to add posts to their favorites list. Implement the ability to track updates and author activity by sending e-mails to the email.

* Commenting on publications: Under each publication, users can comment on the publication and communicate with each other.

* Author Profiles: Each author must have a profile with personal information, including name, photo, and contact information. The profile must contain a list of the author's published publications.

* Administrative access: Site administrators must be granted access to the administrative panel to manage site moderation, users, and content. Ability to block or delete users who violate site rules.

* Analytics and statistics: Monitoring of user activity for the site administrator and collection of statistical data. Ability to analyze and study popular topics and publications.

## **Mockups**
Articles page
![Articles](https://github.com/holyviktor/BrainWave/assets/83537604/de5278f9-a61b-4cb9-bf74-c6e9826ae1ad)

Profile page
![Profile](https://github.com/holyviktor/BrainWave/assets/83537604/6a7b9bb9-43f6-4b01-b642-db9f0ecd27bc)


## **Site content requirements**

### Common pages for all roles:

Main. Title and name of the site, brief information about the site, block with the latest most popular publications and important news.

About us. The page contains information about the site, its goals, privacy policy, terms of use of the site.

Publications page. Contains a list of all scientific publications available for viewing. There should be filters and sorting by categories, keywords, authors, etc.

Page of a separate publication. There is the title and content of the publication, information about the author of the publication, publication date and category. Implement the ability to add a publication to the list of favorites, provide the ability to download a free publication in pdf format.

Publication payment page. On the page, the user can pay for the publication of the article in order to gain access to the review. If the payment is successful or unsuccessful, a corresponding message should be displayed.

### Additional pages for an unauthorized user:

Authorization page. A form for entering a username and password for entering the site. Link to registration page.

Registration page. The form for registering a new user includes fields for name, e-mail, and password.

Favorite posts. The page contains a list of publications that the user has added to favorites. It should be possible to go to the pages of individual publications.


### Additional pages for authorized users:

Profile. Personal information of the user, such as name, photo, contacts. A list of the author's recent published publications, as well as the ability to edit personal information and password.

My investigations. A list of users who follow the current user, as well as who the user follows. Implement the possibility of switching to the profile of each user.

My publications. A list of the author's published publications with the possibility of switching to individual publication pages.

Recommended Publications: List of authors' publications that the user will follow, as well as recommendations of publications based on the recommendation system.

Post creation page. A form for creating a new scientific publication, including fields for title, content with the ability to edit text formatting, categories, keywords, etc. The author can specify the article as free or paid, and can determine the price.

Post edit page. Page for editing already published publications. Ability to edit title, content and formatting, category, keywords, price.

### Site moderator:

Complaint list page. List of scientific publications with complaints received from users. Ability to sort and filter publications.

Post view page. Ability to approve or reject a publication complaint. The moderator has the ability to delete a publication if it violates the rules of using the site.

### Site administrator

User management page. A list of all registered users of the site, the ability to block or delete users as needed. Implement filtering and searching of users by name, e-mail address.

Post Categories Management Page: Ability to add new categories to classify posts, and edit or delete category names and descriptions.

Analytics view page. The page should contain data on the most active and most popular users, current topics of publications, the number and amount of money received on the site for commissions from paid publications.