const parseButton = document.getElementById("parseButton");
const notLinkedInPageError = document.getElementById("notLinkedInPageError");
const linkedInUrlPattern = /^https:\/\/(www\.)?linkedin.com\/in\/[^/]+\/?$/;

function isLinkedInTab(tab) {
    return linkedInUrlPattern.test(tab.url);
}

function showNotLinkedInError() {
    notLinkedInPageError.style.display = "block";
    setTimeout(hideNotLinkedInError, 3000);
}

function hideNotLinkedInError() {
    notLinkedInPageError.style.display = "none";
}

// This function can't be splitted, because it is executed in LinkedIn context
function processLinkedInPage() {
    // window of LinkedIn page

    const mode = "development"; // TODO: add frontend domain and change to "production"
    const frontendUrl = "http://develop.bsa21-scout.com/applicants"; // TODO: Change to real url

    const leftMainInfoPanel = document.querySelector(".pv-text-details__left-panel");
    const rightMainInfoPanel = document.querySelector(".pv-text-details__right-panel");
    const experienceNamesElements = document.querySelectorAll("#experience-section .pv-entity__summary-info--background-section");
    const experienceElements = document.querySelectorAll("#experience-section .pv-entity__date-range.t-14.t-black--light.t-normal");
    const educationElements = document.querySelectorAll("#education-section .pv-entity__summary-info--background-section");
    const skillElements = document.querySelectorAll(".pv-skill-categories-section .pv-skill-category-entity__name-text");
    const emailElement = document.querySelector(".artdeco-modal .t-14.t-black.t-normal");
    const linkElements = document.querySelector(".artdeco-modal .pv-contact-info__contact-link");
    const modalLinkElement = document.querySelector(".pv-text-details__separator .link-without-visited-state");
    const linkedInUrl = window.location.href;

    const [firstName, lastName] = leftMainInfoPanel.children[0].children[0].innerText.split(" ");
    const currentJob = leftMainInfoPanel.children[1].innerText;
    const region = leftMainInfoPanel.children[leftMainInfoPanel.childElementCount - 1].children[0].innerText;
    const jobsAndEducationNames = [];

    if (rightMainInfoPanel) {
        for (const child of rightMainInfoPanel.children) {
            jobsAndEducationNames.push(child.children[0].children[1].children[0].innerText);
        }
    }

    const experienceNames = [];

    for (const child of experienceNamesElements) {
        if (child.children[0].childElementCount === 0) {
            experienceNames.push(child.children[0].innerText);
        } else {
            experienceNames.push(child.children[0].children[1].innerText);
        }
    }

    let experience = 0;
    let index = 0;
    let experienceDescription = "";

    for (const child of experienceElements) {
        const [fromDate, toDate] = child.children[1].innerText.split(" – ");
        // Be careful, this is not a regular dash (it is longer).      ^

        experienceDescription += `${experienceNames[index]} (${fromDate} - ${toDate}), `;
        index += 1;

        let toNum;

        if (toDate === "Present") {
            toNum = new Date().getFullYear();
        } else {
            toNum = Number(toDate.split(" ").pop());
        }

        const fromNum = Number(fromDate.split(" ").pop());

        if (!isNaN(fromNum) && !isNaN(toNum)) {
            experience += toNum - fromNum;
        }
    }

    experienceDescription = experienceDescription
        .substring(0, experienceDescription.length - 2);

    const education = [];

    for (const child of educationElements) {
        const infoBlock = child.children[0];
        const name = infoBlock.children[0].innerText;

        if (infoBlock.children[1] && infoBlock.children[1].childElementCount > 0) {
            const degreePart1 = infoBlock.children[1].children[1].innerText;
            let degreePart2;

            if (infoBlock.childElementCount > 2) {
                degreePart2 = ", " + infoBlock.children[2].children[1].innerText;
            }

            education.push(`${name} (${degreePart1}${degreePart2})`);
        } else {
            education.push(name);
        }
    }

    const skills = [];

    for (const child of skillElements) {
        skills.push(child.innerText);
    }

    modalLinkElement.click();

    let email, phone;

    const emailElements = document.querySelectorAll(".artdeco-modal .pv-contact-info__contact-link");
    const phoneElement = document.querySelector(".artdeco-modal .t-14.t-black.t-normal");

    for (const element of emailElements) {
        if (element.href.startsWith("mailto:")) {
            email = element.innerText.trim();
            break;
        }
    }

    if (phoneElement) {
        phone = phoneElement.innerText.trim();
    }

    const data = {
        firstName,
        lastName,
        email,
        phone,
        // currentJob,
        // region,
        // jobsAndEducationNames,
        experience,
        experienceDescription,
        // education,
        // skills,
        linkedInUrl,
    };

    const string = JSON.stringify(data);
    const latin1 = unescape(encodeURIComponent(string));
    const base64 = btoa(latin1);

    if (mode === "development") {
        console.log("Data:");
        console.log(data);
    }

    window.open(`${frontendUrl}?data=${base64}`, "_blank");
}

parseButton.addEventListener("click", async () => {
    const [activeTab] = await chrome.tabs.query({ active: true, currentWindow: true });

    if (!isLinkedInTab(activeTab)) {
        return showNotLinkedInError();
    }

    chrome.scripting.executeScript({
        target: { tabId: activeTab.id },
        function: processLinkedInPage,
    });
});
