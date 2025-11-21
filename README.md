<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->

<a id="readme-top"></a>

<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->

<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->

[![Issues][issues-shield]][issues-url][![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
  </a>

<p align="center">
  <a href="" target="_blank">🌐 Website (not avalible yet)</a> |
  <a href="https://github.com/diificult/PalBet/" target="_blank">💻 Project GitHub</a> |
  <a href="https://linkedin.com/in/owenjhowarth" target="_blank">🔗 LinkedIn</a> |
  <a href="https://owenhowarth.co.uk" target="_blank">🎨 Portfolio</a>
</p>

<h3 align="center"> 🎲PalBet </h3>

  <p align="center">
    Ever told your mate "I bet you I could do a kick flip". Probably not but you've probably said something similar. Perhaps its a prediction that takes years to know the outcome, so by the time it comes round to it, you've forgotten! I've created a website where you can do exactly that, and not forget. You don't even have to bet real money, using virtual currency, you don't have to risk anything.
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## 📝 About The Project

I wanted to expand my ASP.NET knowledge, and start implementing some React skills.
The website currently allows users to add other users as friends. Once they become friends, you can create "bets" with others, which will notify

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### 🛠️ Built With

-   C#
-   ASP.NET
-   SQL
-   React

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## ▶️ Getting Started

For the backend: This project currently designed to be runnable within Visual Studios, however you are able to run it using "dotnet run". The backend will bring up SwaggerUI if you need to test or view any endpoints.
For the frontend: cd into ./frontend/ and run "npm run dev".

### 🚀 Installation & Setup

This project has two parts:

Backend: ASP.NET Core Web API (/backend) with SQL Server

Frontend: React (Vite) app (/frontend)

#### 📦 Prerequisites

Visual Studio 2022 (ASP.NET workload)

.NET 6+ SDK

SQL Server

Node.js (LTS) + npm

🔧 Backend (/backend)

```
cd backend
dotnet ef database update   # apply migrations
dotnet run
```

or run via Visual Studio

Ensure your SQL connection string in appsettings.json is valid & when running, and you are in HTTPS mode.
Backend typically runs at https://localhost:7130.

🎨 Frontend (/frontend)

```
cd frontend
npm install
npm run dev
```

Frontend runs at http://localhost:5173.

⚡ Running

Start backend + frontend separately.
Make sure the frontend points to the backend API URL in its config

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->

## 🖥️ Usage

This is the homepage. To use the website, you need to sign-up or login in the left panel. You require a username, email (can use a fake one as there is no verification at this stage) and password.
<img width="1823" height="1322" alt="image" src="https://github.com/user-attachments/assets/38e1528c-bf00-4f77-8290-09da9c5c3f28" />

Once logged in your bar will update with all your account information. <div/>
<img width="342" height="996" alt="image" src="https://github.com/user-attachments/assets/38c0b15a-72df-4438-93bb-3a0b3d91d68d" />

Click on friends page to view friends, requests and requested, and to add new friends
<img width="1566" height="1326" alt="image" src="https://github.com/user-attachments/assets/c7eb5abb-e685-47e4-a2f3-56fa47f0ac5d" />

Click on bets page to create new bets and view all bets participated in, including requests
<img width="1993" height="1324" alt="image" src="https://github.com/user-attachments/assets/b2240b50-8b1b-483d-9ae3-3d9d0d9e3ed0" />

Once on bets page, you can click on create new bet to create a new bet. Fill in these details
<img width="1948" height="1321" alt="image" src="https://github.com/user-attachments/assets/2b9bdd1a-ec42-43f5-83a7-0a875db89f2f" />

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->

## 🗺️ Roadmap

### 0.1 - Bare Minimum Site

-   [x] Website has ability to add friends, create bet with friends and notify users.
-   [x] Adjust predictions stakes to allow manual input of not just coins.
-   [x] Introduce error response on screen.

#### 0.1.1

-   [x] Update bets with a target end date
-   [x] Create detailed bet view screen.

### 0.2 - Groups

-   [x] User can create groups with friends.
-   [x] Group leader can edit settings such as coins for the group.
-   [x] Create a page for viewing groups and group information.
-   [x] Group leader can add and remove players.
-   [x] Group leader can adjust permissions of users.

### 0.3 - Daily Rewards

-   [x] Create daily rewards for the user so they can continue to create coins.
-   [x] Create a method to remove coins from the economy to attempt to mitigate inflation risks.

### 0.4 - Improved options for predictions & design

-   [x] Better options for host of predictions, perhaps a draw or failed option, one sided.
-   [x] Create notification when end date reached for a bet - Using hangfire.
-   [ ] Improve page layout and design.

### 0.5 - Real-time

-   [ ] Create more real time to the website, particularly with notifications. Using SignalR.
-   [ ] Introduce caching, most likely redis.

### 0.6 - Account

 -  [ ] Create account details page w/ editing details
 -  [ ] Gets stats from back end
 -  [ ] Display stats from components

### 🎯 STRETCH GOAL - Goals that are out of scope for the moment, but desire to do.

-   [ ] (requires validating) Connect to a sport API to allow friends to bet on sporting events.
-   [ ] Personalisation of avatar and profile.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->

## 👋 Contact Me

Owen - 📧 contact@owenhowarth.co.uk

Project Link: [PalBet](https://github.com/diificult/PalBet/)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->

## Acknowledgments

-   [Read Me Template](https://github.com/othneildrew/Best-README-Template/tree/main)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[issues-shield]: https://img.shields.io/github/issues/github_username/repo_name.svg?style=for-the-badge
[issues-url]: https://github.com/diificult/PalBet/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo_name.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo_name/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/owenjhowarth
[personal-url]: https://owenhowarth.co.uk
[github-url]: https://github.com/diificult/PalBet
